using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public static class ErrorCodes
    {
        public static int ToErrorCode(this ErrorCode error) => (int)error;
        public static bool IsDefinedErrorCode(this int errorCode) => Enum.IsDefined(typeof(ErrorCode), errorCode);
    }
    public enum ErrorCode
    {
        Unknown = 1000,
        CannotSendMessageToThisUser = 50007
    }
}
