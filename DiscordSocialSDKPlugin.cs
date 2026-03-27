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
        public const string VERSION = "1.0.1";
        #endregion

        private void CheckForDiscordLibrary(bool firstTime)
        {
            byte[] Export()
            {
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(LibraryNames.DISCORD_LIBRARY_RESOURE))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            string path = Path.Combine(Path.GetDirectoryName(Info.Location), LibraryNames.DISCORD_LIBRARY_NAME_DLL);
            
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
            string path = Path.Combine(Path.GetDirectoryName(Info.Location), LibraryNames.SYSTEM_MEMORY_NAME_DLL);

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
                    throw new Exception($"Could not load {LibraryNames.SYSTEM_MEMORY_NAME}\n");

                Logger.LogWarning($"Could not load {LibraryNames.SYSTEM_MEMORY_NAME}. Trying to export it from dll...");
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(LibraryNames.SYSTEM_MEMORY_RESOURCE))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        File.WriteAllBytes(path, ms.ToArray());
                        Logger.LogInfo($"Exported {LibraryNames.SYSTEM_MEMORY_NAME} library to {path}");
                    }
                }
                CheckForSystemMemoryLibrary(false);
            }
        }
        private void CheckForUnsafeLibrary(bool firstTime)
        {
            string path = Path.Combine(Path.GetDirectoryName(Info.Location), LibraryNames.SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL);

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
                    throw new Exception($"Could not load {LibraryNames.SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL}\n");

                Logger.LogWarning($"Could not load {LibraryNames.SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL}. Trying to export it from dll...");
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(LibraryNames.SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_RESOURCE))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        File.WriteAllBytes(path, ms.ToArray());
                        Logger.LogInfo($"Exported {LibraryNames.SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL} library to {path}");
                    }
                }
                CheckForUnsafeLibrary(false);
            }
        }


        private void Awake()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Discord Social SDK is only supported on Windows");

            CheckForSystemMemoryLibrary(true);
            CheckForUnsafeLibrary(true);
            CheckForDiscordLibrary(true);

            Initialize();
        }


        private static void Initialize()
        {
            ClientWrapper clientWrapper = new ClientWrapper(1018479644946747522);
            clientWrapper.GetMessage(123).Metadata(); // Возвращает мету дату

            DontDestroyOnLoad(new GameObject("DiscordSocialSDKController", [typeof(DiscordController)]));
        }
    }
}
