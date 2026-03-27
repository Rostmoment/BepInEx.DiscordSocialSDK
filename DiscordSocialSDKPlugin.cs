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
        public const string VERSION = "1.1.0";
        #endregion


        internal static DiscordSocialSDKPlugin Instance {  get; private set; }


        private void Awake()
        {
            Instance = this;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Discord Social SDK is only supported on Windows");


            Libraries.CheckSystemMemoryLibrary();
            Libraries.CheckUnsafeLibrary();
            Libraries.CheckNumericsVectorsLibrary();
            Libraries.CheckDiscordLibrary();

            Initialize();
        }


        private static void Initialize()
        {
            DontDestroyOnLoad(new GameObject("DiscordSocialSDKController", [typeof(DiscordController)]));
        }
    }
}
