using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    class CannotSendMessageToThisUserException : Exception
    {
        public ulong UserID { get; }
        public CannotSendMessageToThisUserException(ulong userId) : base($"Cannot send message to user with id {userId}")
        {
            UserID = userId;
        }
    }
}
