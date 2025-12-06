using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BepInEx.DiscordSocialSDK
{
    [BepInPlugin("rost.moment.unity.bepinex.discordsocialsdk", "Discord Social SDK For BepInEx", "1.0.0")]
    public class DiscoerdSDKPlugin : BaseUnityPlugin
    {
        public const string DISCORD_LIBRARY_NAME = "discord_partner_sdk";
        public const string DISCORD_LIBRARY_NAME_DLL = DISCORD_LIBRARY_NAME + ".dll";

        public const string SYSTEM_MEMORY_NAME = "System.Memory";
        public const string SYSTEM_MEMRY_NAME_DLL = SYSTEM_MEMORY_NAME + ".dll";

        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME = "System.Runtime.CompilerServices.Unsafe";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL = SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME + ".dll";

        private static readonly string[] dependencies = [DISCORD_LIBRARY_NAME_DLL, SYSTEM_MEMRY_NAME_DLL, SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL];

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
                Span<byte> spans = new Span<byte>();

            }
            catch (Exception e)
            { 
                Logger.LogError($"Failed to load System.Memory: {e}");
                throw;
            }

            Logger = base.Logger;
        }
    }
}
