using BepInEx.DiscordSocialSDK.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    class CannotSendMessageToThisUserException : BaseDiscordException
    {
        public ulong UserID { get; }
        public CannotSendMessageToThisUserException(ClientResult result, ulong userId) : base(result, $"Cannot send message to user with id {userId}")
        {
            UserID = userId;
        }
    }
}
