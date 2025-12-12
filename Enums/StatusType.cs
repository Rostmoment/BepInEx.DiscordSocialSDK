using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Enum that specifies the various online statuses for a user.
    /// </summary>
    /// <remarks>
    ///  Generally a user is online or offline, but in Discord users are able to further customize their
    ///  status such as turning on "Do not Disturb" mode or "Dnd" to silence notifications.
    ///
    /// </remarks>
    public enum StatusType
    {
        Online = 0,
        Offline = 1,
        Blocked = 2,
        Idle = 3,
        Dnd = 4,
        Invisible = 5,
        Streaming = 6,
        Unknown = 7,
    }

    public static class StatusTypeExtensions
    {
        private static readonly StatusType[] settableStatuses = [StatusType.Idle, StatusType.Invisible, StatusType.Online, StatusType.Dnd];
        public static bool CanSetAsOwnStatus(this StatusType status) => settableStatuses.Contains(status);
        
    }
}
