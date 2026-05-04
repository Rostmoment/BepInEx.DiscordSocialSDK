using BepInEx.DiscordSocialSDK.Auth;
using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Exceptions;
using BepInEx.DiscordSocialSDK.Handles;
using BepInEx.DiscordSocialSDK.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BepInEx.DiscordSocialSDK.Client
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


        #region get/set something methods
        public MessageHandle GetMessage(ulong messageId) => Client.GetMessageHandle(messageId);
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
            Client.UpdateStatusCallback errorHandle = (result) =>
            {
                if (!result.Successful() && result.ErrorCode() == ErrorCode.InvalidBody.ToErrorCode())
                    throw new InvalidStatusException(result, status);
            };

            Client.SetOnlineStatus(status, errorHandle + callback);
        }

        /// <summary>
        /// Sets the online status for the current client and invokes a callback upon completion
        /// <para>Valid statuses: <see cref="StatusType.Online"/>, <see cref="StatusType.Dnd"/>, <see cref="StatusType.Idle"/>, <see cref="StatusType.Invisible"/></para>
        /// </summary>
        /// <param name="status">The new online status to set for the client</param>
        /// <exception cref="InvalidStatusException">Thrown when status cannot be set as own status</exception>
        public void SetOnlineStatus(StatusType status) => SetOnlineStatus(status, (x) => { });
        #endregion

        #region messages

        /// <summary>
        /// Sends message to user with given ID, text and metadata
        /// </summary>
        /// <param name="userID">User ID to send the message to</param>
        /// <param name="text">Text of the message</param>
        /// <param name="metadata">Metadata associated with the message, just key-value pairs as string</param>
        /// <exception cref="CannotSendMessageToThisUserException">Thrown when the message cannot be sent to the specified user</exception>
        /// <exception cref="CannotSendEmptyMessageException">Thrown when the message text is empty</exception>
        /// <exception cref="UknownDiscordException">Thrown when an unknown error occurs while sending the message</exception>
        public void SendMessageToUser(ulong userID, string text, Dictionary<string, string> metadata) =>
            SendMessageToUser(userID, text, metadata, (x, y) => { });

        /// <summary>
        /// Sends message to user with given ID, text and metadata, and invokes a callback upon completion
        /// </summary>
        /// <param name="userID">User ID to send the message to</param>
        /// <param name="text">Text of the message</param>
        /// <param name="metadata">Metadata associated with the message, just key-value pairs as string</param>
        /// <param name="callback">A callback method to be invoked after the message is sent</param>
        /// <exception cref="CannotSendMessageToThisUserException">Thrown when the message cannot be sent to the specified user</exception>
        /// <exception cref="CannotSendEmptyMessageException">Thrown when the message text is empty</exception>
        /// <exception cref="UknownDiscordException">Thrown when an unknown error occurs while sending the message</exception>
        public void SendMessageToUser(ulong userID, string text, Dictionary<string, string> metadata, Client.SendUserMessageCallback callback)
        {
            Client.SendUserMessageCallback errorHandle = (clientResult, messageID) =>
            {
                if (!clientResult.Successful())
                {
                    int errorCode = clientResult.ErrorCode();
                    if (errorCode == ErrorCode.CannotSendMessageToThisUser.ToErrorCode())
                        throw new CannotSendMessageToThisUserException(clientResult, userID);
                    else if (errorCode == ErrorCode.CannotSendEmptyMessage.ToErrorCode())
                        throw new CannotSendEmptyMessageException(clientResult, userID);
                    else
                        throw new UknownDiscordException(clientResult);
                }
            };

            Client.SendUserMessageCallback resultCallback = errorHandle + callback;

            if (metadata == null || metadata.Count == 0)
                Client.SendUserMessage(userID, text, resultCallback);
            else
                Client.SendUserMessageWithMetadata(userID, text, metadata, resultCallback);
        }


        /// <summary>
        /// Sends a message to a user with multiple serializable metadata objects.
        /// <para>Same as <see cref="SendMessageToUser(ulong, string, Dictionary{string, string})"/> but merges metadata from an array of interfaces.</para>
        /// </summary>
        /// <param name="userID">User ID to send the message to</param>
        /// <param name="text">Text of the message</param>
        /// <param name="metadataObjects">An array of objects implementing <see cref="IDiscordMetaDataSerializable"/></param>
        /// <exception cref="ArgumentException">Thrown when multiple metadata objects contain the same key</exception>
        /// <exception cref="CannotSendMessageToThisUserException">Thrown when the message cannot be sent to the specified user</exception>
        /// <exception cref="CannotSendEmptyMessageException">Thrown when the message text is empty</exception>
        /// <exception cref="UknownDiscordException">Thrown when an unknown error occurs while sending the message</exception>
        public void SendMessageToUser(ulong userID, string text, params IDiscordMetaDataSerializable[] metadataObjects) =>
            SendMessageToUser(userID, text, metadataObjects, (x, y) => { });

        /// <summary>
        /// Sends a message to a user with multiple serializable metadata objects and invokes a callback upon completion.
        /// </summary>
        /// <param name="userID">User ID to send the message to</param>
        /// <param name="text">Text of the message</param>
        /// <param name="metadataObjects">An array of objects implementing <see cref="IDiscordMetaDataSerializable"/></param>
        /// <param name="callback">A callback method to be invoked after the message is sent</param>
        /// <exception cref="ArgumentException">Thrown when multiple metadata objects contain the same key</exception>
        /// <exception cref="CannotSendMessageToThisUserException">Thrown when the message cannot be sent to the specified user</exception>
        /// <exception cref="CannotSendEmptyMessageException">Thrown when the message text is empty</exception>
        /// <exception cref="UknownDiscordException">Thrown when an unknown error occurs while sending the message</exception>
        public void SendMessageToUser(ulong userID, string text, IDiscordMetaDataSerializable[] metadataObjects, Client.SendUserMessageCallback callback)
        {
            Dictionary<string, string> mergedMetadata = new Dictionary<string, string>();

            if (metadataObjects != null)
            {
                foreach (IDiscordMetaDataSerializable obj in metadataObjects)
                {
                    if (obj == null) 
                        continue;

                    Dictionary<string, string> data = obj.Serialize();
                    if (data == null) 
                        continue;

                    foreach (var kvp in data)
                    {
                        if (mergedMetadata.ContainsKey(kvp.Key))
                            throw new ArgumentException($"Duplicate metadata key found: '{kvp.Key}'. Multiple metadata objects cannot share the same key.");

                        mergedMetadata.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            SendMessageToUser(userID, text, mergedMetadata, callback);
        }


        /// <summary>
        /// Opens a message in Discord if possible
        /// </summary>
        /// <param name="messageId">Message id to open</param>
        /// <returns>True if the message was opened successfully, false otherwise</returns>
        public bool OpenMessageInDiscordIfCan(ulong messageId) => OpenMessageInDiscordIfCan(messageId, () => { }, (x) => { });
        /// <summary>
        /// Opens a message in Discord if possible
        /// </summary>
        /// <param name="messageId">Message id to open</param>
        /// <param name="callback">Callback to be called if message is opened successfully</param>
        /// <returns>True if the message was opened successfully, false otherwise</returns>
        public bool OpenMessageInDiscordIfCan(ulong messageId, Client.OpenMessageInDiscordCallback callback) => 
            OpenMessageInDiscordIfCan(messageId, () => { }, callback);

        /// <summary>
        /// Opens a message in Discord if possible
        /// </summary>
        /// <param name="messageId">Message id to open</param>
        /// <param name="provisionalUserMergeRequiredCallback">Callback to be called if a provisional user merge is required</param>
        /// <param name="callback">Callback to be called if message is opened successfully</param>
        /// <returns>True if the message was opened successfully, false otherwise</returns>
        public bool OpenMessageInDiscordIfCan(ulong messageId, Client.ProvisionalUserMergeRequiredCallback provisionalUserMergeRequiredCallback, Client.OpenMessageInDiscordCallback callback)
        {
            if (Client.CanOpenMessageInDiscord(messageId))
            {
                Client.OpenMessageInDiscord(messageId, provisionalUserMergeRequiredCallback, callback);
                return true;
            }
            return false;
        }
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