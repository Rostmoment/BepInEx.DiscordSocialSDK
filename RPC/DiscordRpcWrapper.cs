using BepInEx.DiscordSocialSDK.Enums;
using System;
using System.Drawing.Printing;

namespace BepInEx.DiscordSocialSDK.RPC
{
    public static class DiscordRPCWrapper
    {
        private static Activity currentActivity;
        private static bool initialized;

        /// <summary>
        /// True if wrapper is ready for usage
        /// </summary>
        public static bool IsReady => ClientWrapper.IsReady && initialized;

        /// <summary>
        /// <see cref="ActivityTypes"/> of rpc
        /// </summary>
        public static ActivityTypes ActivityType
        {
            get => currentActivity.Type();
            set => currentActivity.SetType(value);
        }

        /// <summary>
        /// Large image identifier or URL, null if no large image
        /// </summary>
        public static string CurrentLargeImage => currentActivity?.Assets()?.LargeImage();
        /// <summary>
        /// Tooltip string that is shown when the user hovers over the large image, null if no tooltip
        /// </summary>
        public static string CurrentLargeImageText => currentActivity?.Assets()?.LargeText();

        /// <summary>
        /// Small image identifier or URL, secondary image, rendered as a small circle over the large image, null if no small image
        /// </summary>
        public static string CurrentSmallImage => currentActivity?.Assets()?.SmallImage();
        /// <summary>
        /// Tooltip string that is shown when the user hovers over the small image, null if no tooltip
        /// </summary>
        public static string CurrentSmallImageText => currentActivity?.Assets()?.SmallText();

        /// <summary>
        ///  Current name of activity
        /// </summary>
        public static string CurrentName => currentActivity?.Name();
        /// <summary>
        /// Current details that are shown in status, null if no details
        /// </summary>
        public static string CurrentDetails => currentActivity?.Details();
        /// <summary>
        /// Current URL that opens when user clicks details text
        /// </summary>
        public static string CurrentDetailsUrl => currentActivity?.DetailsUrl();
        /// <summary>
        /// Current state that is shown in status, null if no state
        /// </summary>
        public static string CurrentState => currentActivity?.State();
        /// <summary>
        /// Current URL that opens when user clicks state text
        /// </summary>
        public static string CurrentStateUrl => currentActivity?.StateUrl();

        /// <summary>
        /// Id of current party, null if no party
        /// </summary>
        public static string CurrentPartyId => currentActivity?.Party()?.Id();
        /// <summary>
        /// Current size of party, null if no party
        /// </summary>
        public static int? CurrentPartyCurrentSize => currentActivity?.Party()?.CurrentSize();
        /// <summary>
        /// Max size of party, null if no party
        /// </summary>
        public static int? CurrentPartyMaxSize => currentActivity?.Party()?.MaxSize();

        /// <summary>
        /// Start of current timestamp, null if no timestamp
        /// </summary>
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
        /// <summary>
        /// End of current timestamp, null if no timestamp
        /// </summary>
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

        /// <summary>
        /// Current buttons of activity
        /// </summary>
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
        /// Clears current rich presence activity, resets to default state
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

        /// <summary>
        /// Removes rich presence from user's profile
        /// </summary>
        public static void Wipe()
        {
            ClientWrapper.client?.ClearRichPresence();
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
