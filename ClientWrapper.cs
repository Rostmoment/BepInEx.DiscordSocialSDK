using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Exceptions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace BepInEx.DiscordSocialSDK
{
    public static class ClientWrapper
    {

        private static string RefreshToken
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    PlayerPrefs.DeleteKey("RefreshToken");
                else
                    PlayerPrefs.SetString("RefreshToken", value);
            }
            get
            {
                if (PlayerPrefs.HasKey("RefreshToken"))
                    return PlayerPrefs.GetString("RefreshToken");
                return "";
            }
        }

        /// <summary>
        /// <see cref="Client"/> instance
        /// WARNING: Try to don't use this directly, use the wrapper methods instead
        /// </summary>
        public static Client client;
        private static string codeVerifier;

        internal static ulong ApplicationId
        {
            set => client.SetApplicationId(value);
            get => client.GetApplicationId();
        }

        /// <summary>
        /// Status of client
        /// </summary>
        public static Client.Status Status => client == null ? Client.Status.Disconnected : client.GetStatus();
        /// <summary>
        /// True if client is ready
        /// </summary>
        public static bool IsReady => Status == Client.Status.Ready;

        /// <summary>
        /// Online status of user
        /// <para>Can be <see cref="StatusType.Dnd"/>, <see cref="StatusType.Idle"/>, <see cref="StatusType.Online"/>, <see cref="StatusType.Invisible"/></para>
        /// <exception cref="InvalidStatusException">when status is now allowed to be set as own</exception>
        /// </summary>
        public static StatusType OnlineStatus
        {
            set
            {
                if (!value.CanSetAsOwnStatus())
                    throw new InvalidStatusException(value);
                client.SetOnlineStatus(value, (x) => { });
            }
            get => User.Status();

        }
        /// <summary>
        /// Returns hash of client avatar
        /// </summary>
        public static string AvatarHash => User?.Avatar();
        /// <summary>
        /// Username of client
        /// </summary>
        public static string Username => User?.Username();
        /// <summary>
        /// Client user id
        /// </summary>
        public static ulong? Id => User?.Id();
        /// <summary>
        /// Returns <see cref="UserHandle"/> of client
        /// </summary>
        public static UserHandle User => client.GetCurrentUserV2();

        /// <summary>
        /// Invoked whenever a new message is received in either a lobby or a DM. First argument is message id
        /// </summary>
        public static event Client.MessageCreatedCallback onMessageReceived;
        /// <summary>
        /// Invoked whenever a message is deleted. First argument is message id, second is channel id where message was deleted
        /// </summary>
        public static event Client.MessageDeletedCallback onMessageDeleted;
        /// <summary>
        /// Invoked whenever a message is edited. First argument is message id
        /// </summary>
        public static event Client.MessageUpdatedCallback onMessageUpdated;

        /// <summary>
        /// Returns <see cref="MessageHandle"/> of message
        /// </summary>
        /// <param name="messageId">ID of message to get</param>
        /// <returns></returns>
        public static MessageHandle GetMessage(ulong messageId) => client.GetMessageHandle(messageId);

        public static void SendMessageToUser(ulong userID, string text, Dictionary<string, string> metadata) => 
            SendMessageToUser(userID, text, metadata, (x, y) => {});

        /// <summary>
        /// Sends message to user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="text"></param>
        /// <param name="metadata"></param>
        /// <param name="callback"></param>
        public static void SendMessageToUser(ulong userID, string text, Dictionary<string, string> metadata, Client.SendUserMessageCallback callback)
        {
            if (metadata == null || metadata.Count == 0)
                client.SendUserMessage(userID, text, callback);
            else
                client.SendUserMessageWithMetadata(userID, text, metadata, callback);
        }

        internal static void Initialize(ulong appId)
        {
            if (client != null)
                return;

            Logger.LogInfo("Initializing Discord Social SDK Client...");


            client = new Client();
            ApplicationId = appId;

            client.AddLogCallback((message, severity) =>
            {
                Logger.LogError($"{severity}: {message}");
            }, LoggingSeverity.Error);


            Logger.LogInfo("Setting status changed callback...");
            client.SetStatusChangedCallback((status, error, detail) =>
            {
                Logger.LogInfo($"Status: {Client.StatusToString(status)}, Error: {Client.ErrorToString(error)}, Detail: {detail}");
            });

            Logger.LogInfo("Registering launch command...");
            client.RegisterLaunchCommand(ApplicationId, string.Empty);

            if (!string.IsNullOrEmpty(RefreshToken))
            {
                Logger.LogInfo("Using existing refresh token...");
                client.RefreshToken(ApplicationId, RefreshToken, OnRefreshTokenReceived);
            }
            else
            {
                Logger.LogInfo("No refresh token found, starting OAuth flow...");
                StartOAuthFlow();
            }
        }

        private static void StartOAuthFlow()
        {
            try
            {
                AuthorizationCodeVerifier averifierObj = client.CreateAuthorizationCodeVerifier();
                codeVerifier = averifierObj.Verifier();

                AuthorizationArgs args = new AuthorizationArgs();
                args.SetClientId(ApplicationId);
                args.SetScopes(Client.GetDefaultCommunicationScopes());
                args.SetCodeChallenge(averifierObj.Challenge());

                Logger.LogInfo("Starting OAuth flow...");
                client.Authorize(args, AuthCallback);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to start OAuth flow: {ex.Message}");
            }
        }

        private static void AuthCallback(ClientResult result, string code, string redirectUri)
        {
            Logger.LogInfo($"Authorization Result: {result.Successful()}, Code: {code}, RedirectURI: {redirectUri}");

            if (!result.Successful())
            {
                Logger.LogError($"Authorization failed: {result.Error()}");
                return;
            }

            
            client.GetToken(ApplicationId, code, codeVerifier, redirectUri, OnTokenReceived);
        }

        private static void OnRefreshTokenReceived(ClientResult result, string token, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
        {
            if (!result.Successful())
            {
                Logger.LogError($"Refresh token failed: {result.Error()}");
                StartOAuthFlow();
                return;
            }

            OnTokenReceived(result, token, refreshToken, tokenType, expiresIn, scopes);
        }

        private static void OnTokenReceived(ClientResult result, string token, string newRefreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
        {
            if (!result.Successful())
            {
                Logger.LogError($"Token retrieval failed: {result.Error()}");
                return;
            }

            RefreshToken = newRefreshToken;
            Logger.LogInfo($"Refresh token received, length: {RefreshToken?.Length}");

            client.UpdateToken(tokenType, token, OnTokenUpdated);
        }

        private static void OnTokenUpdated(ClientResult result)
        {
            if (!result.Successful())
            {
                Logger.LogError($"Failed to update token: {result.Error()}");
                return;
            }

            Logger.LogInfo("Token updated successfully, connecting...");
            client.Connect();

            client.SetMessageUpdatedCallback(OnMessageUpdated);
            client.SetMessageCreatedCallback(OnMessageCreated);
            client.SetMessageDeletedCallback(OnMessageDeleted);
        }



        private static void OnMessageDeleted(ulong messageId, ulong channelId) => onMessageDeleted?.Invoke(messageId, channelId);
        private static void OnMessageCreated(ulong messageId) => onMessageReceived?.Invoke(messageId);
        private static void OnMessageUpdated(ulong messageId) => onMessageUpdated?.Invoke(messageId);
    }
}