using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BepInEx.DiscordSocialSDK
{
    [BepInPlugin(DiscordSocialSDKPlugin.GUID, DiscordSocialSDKPlugin.NAME, DiscordSocialSDKPlugin.VERSION)]
    public class DiscordSocialSDKPlugin : BaseUnityPlugin
    {
        public const string GUID = "rost.moment.unity.bepinex.discordsocialsdk";
        public const string NAME = "Discord Social SDK For BepInEx";
        public const string VERSION = "1.0.0";

        public const ulong APPLICATION_ID = 1445825505004752898;

        public const string DISCORD_LIBRARY_NAME = "discord_partner_sdk";
        public const string DISCORD_LIBRARY_NAME_DLL = DISCORD_LIBRARY_NAME + ".dll";
        
        public const string SYSTEM_MEMORY_NAME = "System.Memory";
        public const string SYSTEM_MEMORY_NAME_DLL = SYSTEM_MEMORY_NAME + ".dll";

        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME = "System.Runtime.CompilerServices.Unsafe";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL = SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME + ".dll";

        internal static new ManualLogSource Logger;

        private void Awake()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Discord Social SDK is only supported on Windows");

            string path = Path.Combine(Path.GetDirectoryName(Info.Location), DISCORD_LIBRARY_NAME_DLL);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Discord library {DISCORD_LIBRARY_NAME} was not found by path {path}");

            try
            {
                new Span<byte>();
            }
            catch
            { 
                Logger.LogError($"Failed to load {SYSTEM_MEMORY_NAME}");
                throw;
            }

            try
            {
                System.Runtime.CompilerServices.Unsafe.SizeOf<int>();
            }
            catch
            {
                Logger.LogError($"Failed to load {SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME}");
                throw;
            }
            Logger = base.Logger;
            Initialize();

            ClientWrapper.OnlineStatus = Enums.StatusType.Invisible;
        }
        public static void Initialize()
        {
            DontDestroyOnLoad(new GameObject("DiscordSocialSDKController", [typeof(DiscordController)]));
            ClientWrapper.Initialize(APPLICATION_ID);
        }
    }
}
