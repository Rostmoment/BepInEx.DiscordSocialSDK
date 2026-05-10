using BepInEx.DiscordSocialSDK.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public class UnknownDiscordException : BaseDiscordException
    {
        public UnknownDiscordException(ClientResult result) : base(result)
        {
        }
    }
}
