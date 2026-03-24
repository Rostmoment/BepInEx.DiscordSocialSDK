using BepInEx.DiscordSocialSDK.Auth;
using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Exceptions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BepInEx.DiscordSocialSDK
{
    public class ClientWrapper
    {
        public ClientWrapper(ulong appId)
        {
            Initialize(appId);
        }

        #region token
        private string codeVerifier;

        private string RefreshTokenName => $"RefreshToken{ApplicationID}";

        private string RefreshToken
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    PlayerPrefs.DeleteKey(RefreshTokenName);
                else
                    PlayerPrefs.SetString(RefreshTokenName, value);
            }
            get
            {
                if (PlayerPrefs.HasKey(RefreshTokenName))
                    return PlayerPrefs.GetString(RefreshTokenName);
                return "";
            }
        }
        #endregion


        #region properties
        /// <summary>
        /// Status of client
        /// </summary>
        public Client.Status Status => Client == null ? Client.Status.Disconnected : Client.GetStatus();

        /// <summary>
        /// True if client is ready to use
        /// </summary>
        public bool IsReady => Status == Client.Status.Ready;

        /// <summary>
        /// Application id
        /// </summary>
        public ulong ApplicationID { get; private set; }

        /// <summary>
        /// Client
        /// </summary>
        public Client Client { get; private set; }
        #endregion

        public event Client.MessageCreatedCallback onMessageReceived;
        public event Client.MessageDeletedCallback onMessageDeleted;
        public event Client.MessageUpdatedCallback onMessageUpdated;

        #region do something methods
        public MessageHandle GetMessage(ulong messageId) => Client.GetMessageHandle(messageId);

        public void SendMessageToUser(ulong userID, string text, Dictionary<string, string> metadata) =>
            SendMessageToUser(userID, text, metadata, (x, y) => { });

        public void SendMessageToUser(ulong userID, string text, Dictionary<string, string> metadata, Client.SendUserMessageCallback callback)
        {
            Client.SendUserMessageCallback errorHandle = (clientResult, messageID) =>
            {
                if (!clientResult.Successful())
                {
                    if (clientResult.ErrorCode() == ErrorCode.CannotSendMessageToThisUser.ToErrorCode())
                        throw new CannotSendMessageToThisUserException(userID);
                    else
                        throw new UknownException(clientResult);
                }
            };

            Client.SendUserMessageCallback resultCallback = errorHandle + callback;

            if (metadata == null || metadata.Count == 0)
                Client.SendUserMessage(userID, text, resultCallback);
            else
                Client.SendUserMessageWithMetadata(userID, text, metadata, resultCallback);
        }
        #endregion

        #region get/set something methods
        /// <summary>
        /// Returns <see cref="UserHandle"/> of current client
        /// </summary>
        /// <returns></returns>
        public UserHandle GetUser() => Client.GetCurrentUserV2();

        /// <summary>
        /// Returns user ID of client
        /// </summary>
        /// <returns></returns>
        public ulong GetID() => GetUser().Id();

        /// <summary>
        /// Returns username of client
        /// </summary>
        /// <returns></returns>
        public string GetUsername() => GetUser().Username();

        /// <summary>
        /// Returns online status of client
        /// </summary>
        /// <returns></returns>
        public StatusType GetOnlineStatus() => GetUser().Status();

        /// <summary>
        /// Sets the online status for the current client and invokes a callback upon completion
        /// <para>Valid statuses: <see cref="StatusType.Online"/>, <see cref="StatusType.Dnd"/>, <see cref="StatusType.Idle"/>, <see cref="StatusType.Invisible"/></para>
        /// </summary>
        /// <param name="status">The new online status to set for the client</param>
        /// <param name="callback">A callback method to be invoked after the status update operation completes </param>
        /// <exception cref="InvalidStatusException">Thrown when status cannot be set as own status</exception>
        public void SetOnlineStatus(StatusType status, Client.UpdateStatusCallback callback)
        {
            if (!status.CanSetAsOwnStatus())
                throw new InvalidStatusException(status);

            Client.SetOnlineStatus(status, callback);
        }

        /// <summary>
        /// Sets the online status for the current client and invokes a callback upon completion
        /// <para>Valid statuses: <see cref="StatusType.Online"/>, <see cref="StatusType.Dnd"/>, <see cref="StatusType.Idle"/>, <see cref="StatusType.Invisible"/></para>
        /// </summary>
        /// <param name="status">The new online status to set for the client</param>
        /// <exception cref="InvalidStatusException">Thrown when status cannot be set as own status</exception>
        public void SetOnlineStatus(StatusType status) => SetOnlineStatus(status, (x) => { });
        #endregion

        #region initialization
        private void Initialize(ulong appId)
        {
            if (Client != null)
                return;

            Logger.LogInfo("Initializing Discord Social SDK Client...");

            Client = new Client();
            ApplicationID = appId;

            Client.SetApplicationId(ApplicationID);

            Client.AddLogCallback((message, severity) =>
            {
                Logger.LogError($"{severity}: {message}");
            }, LoggingSeverity.Error);

            Client.SetStatusChangedCallback((status, error, detail) =>
            {
                Logger.LogInfo($"Status: {Client.StatusToString(status)}, Error: {Client.ErrorToString(error)}, Detail: {detail}");
            });

            Client.RegisterLaunchCommand(ApplicationID, string.Empty);

            if (!string.IsNullOrEmpty(RefreshToken))
                Client.RefreshToken(ApplicationID, RefreshToken, OnRefreshTokenReceived);
            else
                StartOAuthFlow();
        }

        private void StartOAuthFlow()
        {
            try
            {
                AuthorizationCodeVerifier averifierObj = Client.CreateAuthorizationCodeVerifier();
                codeVerifier = averifierObj.Verifier();

                AuthorizationArgs args = new AuthorizationArgs();
                args.SetClientId(ApplicationID);
                args.SetScopes(Client.GetDefaultCommunicationScopes());
                args.SetCodeChallenge(averifierObj.Challenge());

                Client.Authorize(args, AuthCallback);
            }
            catch (Exception ex)
            {
                Logger.LogError($"OAuth error: {ex.Message}");
            }
        }

        private void AuthCallback(ClientResult result, string code, string redirectUri)
        {
            if (!result.Successful())
            {
                Logger.LogError($"Authorization failed: {result.Error()}");
                return;
            }

            Client.GetToken(ApplicationID, code, codeVerifier, redirectUri, OnTokenReceived);
        }

        private void OnRefreshTokenReceived(ClientResult result, string token, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
        {
            if (!result.Successful())
            {
                StartOAuthFlow();
                return;
            }

            OnTokenReceived(result, token, refreshToken, tokenType, expiresIn, scopes);
        }

        private void OnTokenReceived(ClientResult result, string token, string newRefreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
        {
            if (!result.Successful())
                return;

            RefreshToken = newRefreshToken;

            Client.UpdateToken(tokenType, token, OnTokenUpdated);
        }

        private void OnTokenUpdated(ClientResult result)
        {
            if (!result.Successful())
                return;

            Client.Connect();

            Client.SetMessageUpdatedCallback(OnMessageUpdatedInternal);
            Client.SetMessageCreatedCallback(OnMessageCreatedInternal);
            Client.SetMessageDeletedCallback(OnMessageDeletedInternal);
        }

        private void OnMessageDeletedInternal(ulong messageId, ulong channelId) =>
            onMessageDeleted?.Invoke(messageId, channelId);

        private void OnMessageCreatedInternal(ulong messageId) =>
            onMessageReceived?.Invoke(messageId);

        private void OnMessageUpdatedInternal(ulong messageId) =>
            onMessageUpdated?.Invoke(messageId);
        #endregion
    }
}