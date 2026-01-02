using BepInEx.DiscordSocialSDK.Enums;
using System;
using System.Drawing.Printing;

namespace BepInEx.DiscordSocialSDK.RPC
{
    public static class DiscordRPCWrapper
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

        public static string CurrentName => currentActivity?.Name();
        public static string CurrentDetails => currentActivity?.Details();
        public static string CurrentDetailsUrl => currentActivity?.DetailsUrl();
        public static string CurrentState => currentActivity?.State();
        public static string CurrentStateUrl => currentActivity?.StateUrl();

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

        public static ActivityButton[] CurrentButtons => currentActivity?.GetButtons();

        private static void CreateNewActivity()
        {
            currentActivity?.Dispose();
            currentActivity = new Activity();
            currentActivity.SetApplicationId(ClientWrapper.ApplicationId);
        }
        internal static void Initialize()
        {
            if (initialized)
                return;

            CreateNewActivity();
            ActivityType = ActivityTypes.Playing;

            initialized = true;
        }

        /// <summary>
        /// Sets rich presence info
        /// </summary>
        /// <param name="name">Name of rich presence</param>
        /// <param name="details">Detail for rich presence</param>
        /// <param name="detailsURL">URL that opens when user clicks details text</param>
        /// <param name="state">State for rich presence</param>
        /// <param name="stateURL">URL that opens when user clicks state text</param>
        public static void SetInfo(string name, string details, string detailsURL = null, string state = null, string stateURL = null)
        {
            if (!IsReady)
                return;

            currentActivity.SetName(name);
            currentActivity.SetDetails(details);
            currentActivity.SetDetailsUrl(detailsURL);
            currentActivity.SetState(state);
            currentActivity.SetStateUrl(stateURL);

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

            if (!string.IsNullOrEmpty(largeText))
                assets.SetLargeText(largeText);

            if (!string.IsNullOrEmpty(smallImage))
            {
                assets.SetSmallImage(smallImage);

                if (!string.IsNullOrEmpty(smallText))
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

            CreateNewActivity();

            Update();
        }

        /// <summary>
        /// Adds button to current rich presence
        /// <para>Won't add button if there is already 2 buttons</para>
        /// </summary>
        /// <param name="label">Label for button</param>
        /// <param name="url">URL that opens when user clicks button</param>
        public static void AddButton(string label, string url)
        {
            if (!IsReady)
                return;

            if (CurrentButtons != null && CurrentButtons.Length >= 2)
                return;

            ActivityButton button = new ActivityButton();
            button.SetLabel(label);
            button.SetUrl(url);
            currentActivity.AddButton(button);

            Update();
        }
        // Why discord didn't make a ClearButtons method is beyond me
        /// <summary>
        /// Clears all buttons from current rich presence
        /// </summary>
        public static void ClearButtons()
        {
            if (!IsReady)
                return;

            Activity newActivity = new Activity();

            newActivity.SetApplicationId(ClientWrapper.ApplicationId);
            newActivity.SetType(ActivityType);

            if (!string.IsNullOrEmpty(CurrentName))
                newActivity.SetName(CurrentName);

            if (!string.IsNullOrEmpty(CurrentDetails))
                newActivity.SetDetails(CurrentDetails);

            if (!string.IsNullOrEmpty(CurrentDetailsUrl))
                newActivity.SetDetailsUrl(CurrentDetailsUrl);

            if (!string.IsNullOrEmpty(CurrentState))
                newActivity.SetState(CurrentState);

            if (!string.IsNullOrEmpty(CurrentStateUrl))
                newActivity.SetStateUrl(CurrentStateUrl);

            if (currentActivity.Assets() != null)
                newActivity.SetAssets(currentActivity.Assets());
            
            if (currentActivity.Party() != null)
                newActivity.SetParty(currentActivity.Party());

            if (currentActivity.Timestamps() != null)
                newActivity.SetTimestamps(currentActivity.Timestamps());

            if (currentActivity.Secrets() != null)
                newActivity.SetSecrets(currentActivity.Secrets());


            currentActivity.Dispose();
            currentActivity = newActivity;

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
