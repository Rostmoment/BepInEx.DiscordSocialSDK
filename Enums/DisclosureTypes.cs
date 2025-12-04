using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    // TODO: Add link to MessageHandle documentation when available
    /// <summary>
    ///  Enum that represents various informational disclosures that Discord may make to users, so that
    ///  the game can identity them and customize their rendering as desired.
    /// </summary>
    /// <remarks>
    ///  See MessageHandle for more details.
    ///
    /// </remarks>
    public enum DisclosureTypes
    {
        MessageDataVisibleOnDiscord = 3,
    }
}
