using BepInEx;
using BepInEx.Logging;
using System.Runtime.InteropServices;

namespace BepInEx.DiscordSocialSDK
{
    [BepInPlugin("rost.moment.unity.bepinex.discordsocialsdk", "Discord Social SDK For BepInEx", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        private void Awake()
        {
            Logger = base.Logger;
        }
    }
}
