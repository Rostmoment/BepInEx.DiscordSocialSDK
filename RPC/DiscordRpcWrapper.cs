using BepInEx.DiscordSocialSDK.Client;
using BepInEx.DiscordSocialSDK.Enums;
using System;

namespace BepInEx.DiscordSocialSDK.RPC
{
    public class DiscordRPCWrapper
    {
        public DiscordRPCWrapper(ClientWrapper client)
        {
            this.client = client;
            Initialize();
        }

        private ClientWrapper client;
        private Activity noButtons;
        private Activity currentActivity;
        private bool initialized;

        /// <summary>
        /// True if wrapper is ready for usage
        /// </summary>
        public bool IsReady => client.IsReady && initialized;

        /// <summary>
        /// <see cref="ActivityTypes"/> of rpc, null if no activity
        /// </summary>
        public ActivityTypes? ActivityType => currentActivity?.Type();

        /// <summary>
        /// Large image identifier or URL, null if no large image
        /// </summary>
        public string CurrentLargeImage => currentActivity?.Assets()?.LargeImage();
        /// <summary>
        /// Tooltip string that is shown when the user hovers over the large image, null if no tooltip
        /// </summary>
        public string CurrentLargeImageText => currentActivity?.Assets()?.LargeText();

        /// <summary>
        /// Small image identifier or URL, secondary image, rendered as a small circle over the large image, null if no small image
        /// </summary>
        public string CurrentSmallImage => currentActivity?.Assets()?.SmallImage();
        /// <summary>
        /// Tooltip string that is shown when the user hovers over the small image, null if no tooltip
        /// </summary>
        public string CurrentSmallImageText => currentActivity?.Assets()?.SmallText();

        /// <summary>
        ///  Current name of activity
        /// </summary>
        public string CurrentName => currentActivity?.Name();
        /// <summary>
        /// Current details that are shown in status, null if no details
        /// </summary>
        public string CurrentDetails => currentActivity?.Details();
        /// <summary>
        /// Current URL that opens when user clicks details text
        /// </summary>
        public string CurrentDetailsUrl => currentActivity?.DetailsUrl();
        /// <summary>
        /// Current state that is shown in status, null if no state
        /// </summary>
        public string CurrentState => currentActivity?.State();
        /// <summary>
        /// Current URL that opens when user clicks state text
        /// </summary>
        public string CurrentStateUrl => currentActivity?.StateUrl();

        /// <summary>
        /// Id of current party, null if no party
        /// </summary>
        public string CurrentPartyId => currentActivity?.Party()?.Id();
        /// <summary>
        /// Current size of party, null if no party
        /// </summary>
        public int? CurrentPartyCurrentSize => currentActivity?.Party()?.CurrentSize();
        /// <summary>
        /// Max size of party, null if no party
        /// </summary>
        public int? CurrentPartyMaxSize => currentActivity?.Party()?.MaxSize();
        /// <summary>
        /// <see cref="ActivityPartyPrivacy"/> of current party, null if no party
        /// </summary>
        public ActivityPartyPrivacy? CurrentPartyPrivacy => currentActivity?.Party()?.Privacy();

        /// <summary>
        /// Start of current timestamp, null if no timestamp
        /// </summary>
        public DateTime? CurrentStartTimestamp
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
        public DateTime? CurrentEndTimestamp
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
        public ActivityButton[] CurrentButtons => currentActivity?.GetButtons();

        private void CreateNewActivity()
        {
            currentActivity?.Dispose();
            currentActivity = new Activity();
            currentActivity.SetApplicationId(client.ApplicationID);

            noButtons?.Dispose();
            noButtons = new Activity();
            noButtons.SetApplicationId(client.ApplicationID);
        }
        internal void Initialize()
        {
            if (initialized)
                return;

            CreateNewActivity();

            initialized = true;
        }

        public void SetActivityType(ActivityTypes activityType)
        {
            if (!IsReady)
                return;

            currentActivity.SetType(activityType);
            noButtons.SetType(activityType);

            Update();
        }

        /// <summary>
        /// Sets rich presence info
        /// </summary>
        /// <param name="name">Name of rich presence</param>
        /// <param name="details">Detail for rich presence</param>
        /// <param name="detailsURL">URL that opens when user clicks details text</param>
        /// <param name="state">State for rich presence</param>
        /// <param name="stateURL">URL that opens when user clicks state text</param>
        public void SetInfo(string name, string details, string detailsURL = null, string state = null, string stateURL = null)
        {
            if (!IsReady)
                return;

            currentActivity.SetName(name);
            currentActivity.SetDetails(details);
            currentActivity.SetDetailsUrl(detailsURL);
            currentActivity.SetState(state);
            currentActivity.SetStateUrl(stateURL);

            noButtons.SetName(name);
            noButtons.SetDetails(details);
            noButtons.SetDetailsUrl(detailsURL);
            noButtons.SetState(state);
            noButtons.SetStateUrl(stateURL);

            Update();
        }


        /// <summary>
        /// Sets rich presence assets
        /// </summary>
        /// <param name="largeImage">Large image identifier or URL</param>
        /// <param name="largeText">Tooltip string that is shown when the user hovers over the large image</param>
        /// <param name="smallImage">The secondary image, rendered as a small circle over the large image</param>
        /// <param name="smallText">Tooltip string that is shown when the user hovers over the small image</param>
        public void SetAssets(string largeImage, string largeText = null, string smallImage = null, string smallText = null)
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
            noButtons.SetAssets(assets);

            Update();

        }


        /// <summary>
        /// Sets party info
        /// </summary>
        public void SetParty(string partyId, int current, int max, ActivityPartyPrivacy privacy)
        {
            if (!IsReady)
                return;

            ActivityParty party = new ActivityParty();
            party.SetId(partyId);
            party.SetCurrentSize(current);
            party.SetMaxSize(max);
            party.SetPrivacy(privacy);

            currentActivity.SetParty(party);
            noButtons.SetParty(party);

            Update();
        }

        /// <summary>
        /// Sets activity timestamps using DateTime values.
        /// </summary>
        public void SetTimestamps(DateTime? start = null, DateTime? end = null)
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
                timestamps.SetStart(start.Value);

            if (end != null)
                timestamps.SetEnd(end.Value);

            currentActivity.SetTimestamps(timestamps);
            noButtons.SetTimestamps(timestamps);

            Update();
        }
        /// <summary>
        /// Starts activity timer from now
        /// </summary>
        public void StartTimerNow() => SetTimestamps(DateTime.UtcNow);

        /// <summary>
        /// Clears activity timestamps
        /// </summary>
        public void ClearTimestamps()
        {
            if (!IsReady)
                return;

            currentActivity.SetTimestamps(null);
            noButtons.SetTimestamps(null);

            Update();
        }

        /// <summary>
        /// Clears current rich presence activity, resets to default state
        /// </summary>
        public void Clear()
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
        public void AddButton(string label, string url)
        {
            ActivityButton button = new ActivityButton();
            button.SetLabel(label);
            button.SetUrl(url);
            AddButton(button);
        }
        /// <summary>
        /// Adds button to current rich presence
        /// <para>Won't add button if there is already 2 buttons</para>
        /// </summary>
        /// <param name="button">Button to add</param>
        public void AddButton(ActivityButton button)
        {
            if (!IsReady)
                return;

            if (CurrentButtons != null && CurrentButtons.Length >= 2)
                return;
            currentActivity.AddButton(button);
            // Buttons cannot be removed from activity, that's why I made noButtons field

            Update();
        }

        // Why discord didn't make a ClearButtons method is beyond me
        /// <summary>
        /// Clears all buttons from current rich presence
        /// </summary>
        public void ClearButtons()
        {
            if (!IsReady)
                return;

            currentActivity?.Dispose();
            currentActivity = new Activity(noButtons);

            Update();
        }

        /// <summary>
        /// Removes rich presence from user's profile
        /// </summary>
        public void Wipe()
        {
            client.Client?.ClearRichPresence();
        }

        private void Update()
        {
           client.Client?.UpdateRichPresence(currentActivity, Callback);
        }

        private void Callback(ClientResult result)
        {
            if (!result.Successful())
                Logger.LogError($"Failed to update Discord Rich Presence: {result.Error()}");
        }
    }
}
