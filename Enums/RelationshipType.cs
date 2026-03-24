using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Enum that represents the possible types of relationships that can exist between two users
    /// </summary>
    public enum RelationshipType
    {
        None = 0,
        Friend = 1,
        Blocked = 2,
        PendingIncoming = 3,
        PendingOutgoing = 4,
        Implicit = 5,
        Suggestion = 6,
    }
}
