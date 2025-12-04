using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Represents the type of additional content contained in a message.
    /// </summary>
    public enum AdditionalContentType
    {
        Other = 0,
        Attachment = 1,
        Poll = 2,
        VoiceMessage = 3,
        Thread = 4,
        Embed = 5,
        Sticker = 6,
    }
}
