using BepInEx.DiscordSocialSDK.Client;
using BepInEx.DiscordSocialSDK.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public class InvalidStatusException : BaseDiscordException
    {
        public StatusType Status { get; }
        public InvalidStatusException(ClientResult result, StatusType status) : base(result, $"The status '{status}' is not a valid status for this operation")
        {
            Status = status;
        }
    }
}
