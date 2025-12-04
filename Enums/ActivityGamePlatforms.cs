using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Represents the type of platforms that an activity invite can be accepted on.
    /// </summary>
    [Flags]
    public enum ActivityGamePlatforms
    {
        Desktop = 1,
        Xbox = 2,
        Samsung = 4,
        IOS = 8,
        Android = 16,
        Embedded = 32,
        PS4 = 64,
        PS5 = 128,
    }
}
