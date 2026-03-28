using BepInEx.DiscordSocialSDK.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public abstract class BaseDiscordException : Exception
    {
        public ClientResult Result { get; }

        public BaseDiscordException(ClientResult result) : base(result.Error())
        {
            Result = result;
        }

        public BaseDiscordException(ClientResult result, string message) : base(message)
        {
            Result = result;
        }

        public override string ToString()
        {
            if (Result == null)
                return Message;

            return $"{Message}\n" +
                   $"Type: {Result.Type()}\n" +
                   $"Code: {Result.ErrorCode()}\n" +
                   $"Status: {Result.Status()}\n" +
                   $"Body: {Result.ResponseBody()}";
        }
    }
}
