using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.DiscordSocialSDK.RPC;
using BepInEx.Logging;
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
        public const string GUID = "rost.moment.unity.bepinex.discordsocialsdk";
        public const string NAME = "Discord Social SDK For BepInEx";
        public const string VERSION = "1.0.1";

        public const ulong APPLICATION_ID = 1445825505004752898;

        public const string DISCORD_LIBRARY_NAME = "discord_partner_sdk";
        public const string DISCORD_LIBRARY_NAME_DLL = DISCORD_LIBRARY_NAME + ".dll";
        public const string DISCORD_LIBRARY_RESOURE = "BepInEx.DiscordSocialSDK.DLLs." + DISCORD_LIBRARY_NAME_DLL;

        public const string SYSTEM_MEMORY_NAME = "System.Memory";
        public const string SYSTEM_MEMORY_NAME_DLL = SYSTEM_MEMORY_NAME + ".dll";
        public const string SYSTEM_MEMORY_RESOURCE = "BepInEx.DiscordSocialSDK.DLLs." + SYSTEM_MEMORY_NAME_DLL;

        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME = "System.Runtime.CompilerServices.Unsafe";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL = SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME + ".dll";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_RESOURCE = "BepInEx.DiscordSocialSDK.DLLs." + SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL;

        internal static new ManualLogSource Logger;

        private void CheckForDiscordLibrary(bool firstTime)
        {
            byte[] Export()
            {
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(DISCORD_LIBRARY_RESOURE))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            string path = Path.Combine(Path.GetDirectoryName(Info.Location), DISCORD_LIBRARY_NAME_DLL);
            
            if (!File.Exists(path))
            {
                if (!firstTime)
                    throw new FileNotFoundException($"File {path} not found, cannot load Discord Social Sdk");

                Logger.LogWarning("Discord library was not found. Trying to export it from dll...");
                File.WriteAllBytes(path, Export());
                Logger.LogInfo($"Exported discord library to {path}");
                CheckForDiscordLibrary(false);
            }
            else
            {
                if (!Export().SequenceEqual(File.ReadAllBytes(path)))
                {
                    Logger.LogWarning("Discord library differs from the embedded one. Re-exporting...");
                    File.WriteAllBytes(path, Export());
                }
            }
        }
        private void CheckForSystemMemoryLibrary(bool firstTime)
        {
            string path = Path.Combine(Path.GetDirectoryName(Info.Location), SYSTEM_MEMORY_NAME_DLL);

            bool needExport = false;

            try
            {
                Type t = Type.GetType("System.Span`1, System.Memory");
                if (t == null)
                    needExport = true;
            }
            catch
            {
                needExport = true;
            }

            if (needExport)
            {
                if (!firstTime)
                    throw new Exception($"Could not load {SYSTEM_MEMORY_NAME}\n");

                Logger.LogWarning($"Could not load {SYSTEM_MEMORY_NAME}. Trying to export it from dll...");
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(SYSTEM_MEMORY_RESOURCE))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        File.WriteAllBytes(path, ms.ToArray());
                        Logger.LogInfo($"Exported {SYSTEM_MEMORY_NAME} library to {path}");
                    }
                }
                CheckForSystemMemoryLibrary(false);
            }
        }
        private void CheckForUnsafeLibrary(bool firstTime)
        {
            string path = Path.Combine(Path.GetDirectoryName(Info.Location), SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL);

            bool needExport = false;

            try
            {
                Type t = Type.GetType("System.Runtime.CompilerServices.Unsafe, System.Runtime.CompilerServices.Unsafe");
                if (t == null)
                    needExport = true;
            }
            catch
            {
                needExport = true;
            }

            if (needExport)
            {
                if (!firstTime)
                    throw new Exception($"Could not load {SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL}\n");

                Logger.LogWarning($"Could not load {SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL}. Trying to export it from dll...");
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_RESOURCE))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        File.WriteAllBytes(path, ms.ToArray());
                        Logger.LogInfo($"Exported {SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL} library to {path}");
                    }
                }
                CheckForUnsafeLibrary(false);
            }
        }


        private void Awake()
        {
            Logger = base.Logger;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Discord Social SDK is only supported on Windows");

            CheckForDiscordLibrary(true);
            CheckForSystemMemoryLibrary(true);
            CheckForUnsafeLibrary(true);

            Initialize();

            ClientWrapper.OnlineStatus = Enums.StatusType.Invisible;
        }
        public static void Initialize()
        {
            DontDestroyOnLoad(new GameObject("DiscordSocialSDKController", [typeof(DiscordController)]));

            ClientWrapper.Initialize(APPLICATION_ID);
            DiscordRPCWrapper.Initialize();
        }
    }
}
