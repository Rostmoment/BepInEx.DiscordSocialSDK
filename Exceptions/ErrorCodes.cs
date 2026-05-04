using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Exceptions
{
    public static class ErrorCodeExtensions
    {

        public static int ToErrorCode(this ErrorCode error) => (int)error;
        public static bool IsDefinedErrorCode(this int errorCode) => Enum.IsDefined(typeof(ErrorCode), errorCode);
    }
    /// <summary>
    /// Error codes of discord api, you can see full list in official site https://docs.discord.com/developers/topics/opcodes-and-status-codes
    /// </summary>
    public enum ErrorCode
    {
        Unknown = 1000,
        CannotSendEmptyMessage = 50006,
        CannotSendMessageToThisUser = 50007,
        InvalidBody = 50035
    }
}
