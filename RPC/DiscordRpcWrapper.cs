using BepInEx.DiscordSocialSDK.Enums;
using System;

namespace BepInEx.DiscordSocialSDK.RPC
{
    public static class DiscordRpcWrapper
    {
        private static Activity currentActivity;
        private static bool initialized;

        public static bool IsReady => ClientWrapper.IsReady && initialized;
        public static ActivityTypes ActivityType
        {
            get => currentActivity.Type();
            set => currentActivity.SetType(value);
        }

        public static string CurrentLargeImage => currentActivity?.Assets()?.LargeImage();
        public static string CurrentLargeImageText => currentActivity?.Assets()?.LargeText();

        public static string CurrentSmallImage => currentActivity?.Assets()?.SmallImage();
        public static string CurrentSmallImageText => currentActivity?.Assets()?.SmallText();

        public static string CurrentDetails => currentActivity?.Details();
        public static string CurrentState => currentActivity?.State();

        public static string CurrentPartyId => currentActivity?.Party()?.Id();
        public static int? CurrentPartyCurrentSize => currentActivity?.Party()?.CurrentSize();
        public static int? CurrentPartyMaxSize => currentActivity?.Party()?.MaxSize();

        public static DateTime? CurrentStartTimestamp
        {
            get
            {
                ulong? ts = currentActivity?.Timestamps()?.Start();
                if (ts == null)
                    return null;
                return DateTimeOffset.FromUnixTimeMilliseconds((long)ts).UtcDateTime;
            }
        }
        public static DateTime? CurrentEndTimestamp
        {
            get
            {
                ulong? ts = currentActivity?.Timestamps()?.End();
                if (ts == null)
                    return null;
                return DateTimeOffset.FromUnixTimeMilliseconds((long)ts).UtcDateTime;
            }
        }

        internal static void Initialize()
        {
            if (initialized)
                return;

            currentActivity = new Activity();
            currentActivity.SetApplicationId(ClientWrapper.ApplicationId);
            ActivityType = ActivityTypes.Playing;

            initialized = true;
        }

        /// <summary>
        /// Sets rich presence info
        /// </summary>
        /// <param name="details">Detail for rich presence</param>
        /// <param name="state">State for rich presence</param>
        public static void SetInfo(string details, string state = null)
        {
            if (!IsReady)
                return;

            currentActivity.SetDetails(details);
            currentActivity.SetState(state);

            Update();
        }


        /// <summary>
        /// Sets rich presence assets
        /// </summary>
        /// <param name="largeImage">Large image identifier or URL</param>
        /// <param name="largeText">Tooltip string that is shown when the user hovers over the large image</param>
        /// <param name="smallImage">The secondary image, rendered as a small circle over the large image</param>
        /// <param name="smallText">Tooltip string that is shown when the user hovers over the small image</param>
        public static void SetAssets(string largeImage, string largeText = null, string smallImage = null, string smallText = null)
        {
            if (!IsReady)
                return;

            ActivityAssets assets = new ActivityAssets();
            assets.SetLargeImage(largeImage);

            if (!largeText.IsNullOrWhiteSpace())
                assets.SetLargeText(largeText);

            if (!smallImage.IsNullOrWhiteSpace())
            {
                assets.SetSmallImage(smallImage);

                if (!smallText.IsNullOrWhiteSpace())
                    assets.SetSmallText(smallText);
            }
            currentActivity.SetAssets(assets);

            Update();

        }


        /// <summary>
        /// Sets party info
        /// </summary>
        public static void SetParty(string partyId, int current, int max)
        {
            if (!IsReady)
                return;

            ActivityParty party = new ActivityParty();
            party.SetId(partyId);
            party.SetCurrentSize(current);
            party.SetMaxSize(max);

            currentActivity.SetParty(party);
            Update();
        }

        /// <summary>
        /// Sets activity timestamps using DateTime values.
        /// </summary>
        public static void SetTimestamps(DateTime? start = null, DateTime? end = null)
        {
            if (!IsReady)
                return;

            if (start == null && end == null)
            {
                currentActivity.SetTimestamps(null);
                Update();
                return;
            }

            ActivityTimestamps timestamps = new ActivityTimestamps();

            if (start != null)
                timestamps.SetStart((ulong)new DateTimeOffset(start.Value).ToUnixTimeMilliseconds());

            if (end != null)
                timestamps.SetEnd((ulong)new DateTimeOffset(end.Value).ToUnixTimeMilliseconds());

            currentActivity.SetTimestamps(timestamps);
            Update();
        }
        /// <summary>
        /// Starts activity timer from now
        /// </summary>
        public static void StartTimerNow() => SetTimestamps(DateTime.UtcNow);

        /// <summary>
        /// Clears activity timestamps
        /// </summary>
        public static void ClearTimestamps()
        {
            if (!IsReady)
                return;

            currentActivity.SetTimestamps(null);
            Update();
        }

        /// <summary>
        /// Clears current rich presence activity
        /// </summary>
        public static void Clear()
        {
            if (!IsReady)
                return;

            currentActivity.Dispose();
            currentActivity = new Activity();

            Update();
        }


        private static void Update()
        {
            ClientWrapper.client?.UpdateRichPresence(currentActivity, Callback);
        }

        private static void Callback(ClientResult result)
        {
            if (!result.Successful())
                DiscordSocialSDKPlugin.Logger.LogError($"Failed to update Discord Rich Presence: {result.Error()}");
        }
    }
}
