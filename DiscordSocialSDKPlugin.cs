using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.DiscordSocialSDK.Client;
using BepInEx.DiscordSocialSDK.RPC;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BepInEx.DiscordSocialSDK
{
    [BepInPlugin(DiscordSocialSDKPlugin.GUID, DiscordSocialSDKPlugin.NAME, DiscordSocialSDKPlugin.VERSION)]
    public class DiscordSocialSDKPlugin : BaseUnityPlugin
    {
        #region constants
        public const string GUID = "rost.moment.unity.bepinex.discordsocialsdk";
        public const string NAME = "Discord Social SDK For BepInEx";
        public const string VERSION = "1.1.1";

        private const string PING_HOST = "discord.com";
        private const int PING_TIMEOUT_MS = 3000;
        #endregion


        internal static DiscordSocialSDKPlugin Instance { get; private set; }


        private void Awake()
        {
            Instance = this;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Discord Social SDK is only supported on Windows");

            if (!CheckInternetConnection())
                throw new Exception("No internet connection detected. Discord Social SDK requires internet access.");

            Libraries.CheckSystemMemoryLibrary();
            Libraries.CheckUnsafeLibrary();
            Libraries.CheckNumericsVectorsLibrary();
            Libraries.CheckDiscordLibrary();

            Initialize();
        }

        private static bool CheckInternetConnection()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                DiscordSocialSDK.Logger.LogWarning("No active network interfaces found");
                return false;
            }

            DiscordSocialSDK.Logger.LogInfo("Network interface available. Pinging discord.com...");

            try
            {
                using System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                PingReply reply = ping.Send(PING_HOST, PING_TIMEOUT_MS);

                if (reply.Status == IPStatus.Success)
                {
                    DiscordSocialSDK.Logger.LogInfo($"Ping to {PING_HOST} succeeded ({reply.RoundtripTime}ms).");
                    return true;
                }

                DiscordSocialSDK.Logger.LogWarning($"Ping to {PING_HOST} failed: {reply.Status}.");
                return false;
            }
            catch (Exception ex)
            {
                DiscordSocialSDK.Logger.LogWarning($"Ping to {PING_HOST} threw an exception: {ex.Message}");
                return false;
            }
        }


        private static void Initialize()
        {
            DontDestroyOnLoad(new GameObject("DiscordSocialSDKController", [typeof(DiscordController)]));
        }
    }
}