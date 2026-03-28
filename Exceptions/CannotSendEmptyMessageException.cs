using BepInEx.DiscordSocialSDK.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public class CannotSendEmptyMessageException : BaseDiscordException
    {
        public ulong UserId { get; }
        public CannotSendEmptyMessageException(ClientResult result, ulong userId) : base(result, $"Tried to send empty message to user {userId}")
        {
            UserId = userId;
        } 
    }
}
