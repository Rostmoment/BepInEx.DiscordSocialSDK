using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Exceptions;
using System;
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

        internal static Client client;
        private static string codeVerifier;

        public static ulong ApplicationId
        {
            set => client.SetApplicationId(value);
            get => client.GetApplicationId();
        }

        public static Client.Status Status => client == null ? Client.Status.Disconnected : client.GetStatus();
        public static bool IsReady => Status == Client.Status.Ready;
        public static StatusType OnlineStatus
        {
            set
            {
                if (!value.CanSetAsOwnStatus())
                    throw new InvalidStatusException(value);
                client.SetOnlineStatus(value, (x) => { });
            }
            get => client.GetCurrentUser().Status();

        }


        internal static void Initialize(ulong appId)
        {
            if (client != null)
                return;

            DiscordSocialSDKPlugin.Logger.LogInfo("Initializing Discord Social SDK Client...");


            client = new Client();
            ApplicationId = appId;

            client.AddLogCallback((message, severity) =>
            {
                DiscordSocialSDKPlugin.Logger.LogError($"{severity}: {message}");
            }, LoggingSeverity.Error);


            DiscordSocialSDKPlugin.Logger.LogInfo("Setting status changed callback...");
            client.SetStatusChangedCallback((status, error, detail) =>
            {
                DiscordSocialSDKPlugin.Logger.LogInfo($"Status: {Client.StatusToString(status)}, Error: {Client.ErrorToString(error)}, Detail: {detail}");
            });

            DiscordSocialSDKPlugin.Logger.LogInfo("Registering launch command...");
            client.RegisterLaunchCommand(ApplicationId, string.Empty);

            if (!string.IsNullOrEmpty(RefreshToken))
            {
                DiscordSocialSDKPlugin.Logger.LogInfo("Using existing refresh token...");
                client.RefreshToken(ApplicationId, RefreshToken, OnRefreshTokenReceived);
            }
            else
            {
                DiscordSocialSDKPlugin.Logger.LogInfo("No refresh token found, starting OAuth flow...");
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

                DiscordSocialSDKPlugin.Logger.LogInfo("Starting OAuth flow...");
                client.Authorize(args, AuthCallback);
            }
            catch (Exception ex)
            {
                DiscordSocialSDKPlugin.Logger.LogError($"Failed to start OAuth flow: {ex.Message}");
            }
        }

        private static void AuthCallback(ClientResult result, string code, string redirectUri)
        {
            DiscordSocialSDKPlugin.Logger.LogInfo($"Authorization Result: {result.Successful()}, Code: {code}, RedirectURI: {redirectUri}");

            if (!result.Successful())
            {
                DiscordSocialSDKPlugin.Logger.LogError($"Authorization failed: {result.Error()}");
                return;
            }

            
            client.GetToken(ApplicationId, code, codeVerifier, redirectUri, OnTokenReceived);
        }

        private static void OnRefreshTokenReceived(ClientResult result, string token, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
        {
            if (!result.Successful())
            {
                DiscordSocialSDKPlugin.Logger.LogError($"Refresh token failed: {result.Error()}");
                StartOAuthFlow();
                return;
            }

            OnTokenReceived(result, token, refreshToken, tokenType, expiresIn, scopes);
        }

        private static void OnTokenReceived(ClientResult result, string token, string newRefreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
        {
            if (!result.Successful())
            {
                DiscordSocialSDKPlugin.Logger.LogError($"Token retrieval failed: {result.Error()}");
                return;
            }

            RefreshToken = newRefreshToken;
            DiscordSocialSDKPlugin.Logger.LogInfo($"Refresh token received, length: {RefreshToken?.Length}");

            client.UpdateToken(tokenType, token, OnTokenUpdated);
        }

        private static void OnTokenUpdated(ClientResult result)
        {
            if (!result.Successful())
            {
                DiscordSocialSDKPlugin.Logger.LogError($"Failed to update token: {result.Error()}");
                return;
            }

            DiscordSocialSDKPlugin.Logger.LogInfo("Token updated successfully, concting...");
            client.Connect();
        }
    }
}