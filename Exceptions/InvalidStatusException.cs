using BepInEx.DiscordSocialSDK.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    class InvalidStatusException : Exception
    {
        public StatusType Status { get; }
        public InvalidStatusException(StatusType status) : base($"The status '{status}' is not a valid status for this operation")
        {
            Status = status;
        }
    }
}
