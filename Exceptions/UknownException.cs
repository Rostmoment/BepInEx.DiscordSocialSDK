using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public class UknownException : Exception
    {
        public ClientResult ClientResult { get; }
        public UknownException(ClientResult result) : base()
        {
            ClientResult = result;
        }
    }
}
