using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Enum that represents the logical groups of relationships based on online status and game
    ///  activity
    /// </summary>
    public enum RelationshipGroupType
    {
        OnlinePlayingGame = 0,
        OnlineElsewhere = 1,
        Offline = 2,
    }
}
