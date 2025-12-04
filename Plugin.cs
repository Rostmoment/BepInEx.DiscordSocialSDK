using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BepInEx.DiscordSocialSDK
{
    [BepInPlugin("rost.moment.unity.bepinex.discordsocialsdk", "Discord Social SDK For BepInEx", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static readonly string[] dependencies = ["discord_partner_sdk.dll", "System.Memory.dll", "System.Runtime.CompilerServices.Unsafe.dll", "System.Runtime.CompilerServices.Unsafe.dll"];

        internal static new ManualLogSource Logger;

        private void Awake()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("Discord Social SDK is only supported on Windows");

            foreach (string dependency in dependencies)
            {
                string path = Path.Combine(Path.GetDirectoryName(Info.Location), dependency);
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Required dependency '{dependency}' not found at path: {path}");
            }

            Logger = base.Logger;
        }
    }
}
