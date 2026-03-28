using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public class CannotSendEmptyMessageException : Exception
    {
        public ulong UserId { get; }
        public CannotSendEmptyMessageException(ulong userId) : base($"Tried to send empty message to user {userId}")
        {
            UserId = userId;
        } 
    }
}
