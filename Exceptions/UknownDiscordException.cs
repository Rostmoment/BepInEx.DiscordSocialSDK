using BepInEx.DiscordSocialSDK.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public class UknownDiscordException : Exception
    {
        public ClientResult ClientResult { get; }
        public UknownDiscordException(ClientResult result) : base(result.Error())
        {
            ClientResult = result;
        }
    }
}
