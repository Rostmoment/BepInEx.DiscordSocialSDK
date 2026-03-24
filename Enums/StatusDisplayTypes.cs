using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Controls which Discord RichPresence field is displayed in the user's status.
    /// </summary>
    /// <remarks>
    ///  See https://discord.com/developers/docs/rich-presence/overview for more information.
    ///
    /// </remarks>
    public enum StatusDisplayTypes
    {
        Name = 0,
        State = 1,
        Details = 2,
    }
}
