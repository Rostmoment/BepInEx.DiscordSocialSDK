using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BepInEx.DiscordSocialSDK
{
    public static class Logger
    {
        private static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(DiscordSocialSDKPlugin.NAME);

        public static void LogError<T>(T message, [CallerMemberName] string caller = null, [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.LogError($"Error from {caller} method at line {sourceLineNumber}");
            logger.LogError(message);
        }

        public static void LogInfo<T>(T message, [CallerMemberName] string caller = null, [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.LogInfo($"Info from {caller} method at line {sourceLineNumber}");
            logger.LogInfo(message);
        }

        public static void LogWarning<T>(T message, [CallerMemberName] string caller = null, [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.LogWarning($"Warning from {caller} method at line {sourceLineNumber}");
            logger.LogWarning(message);
        }

        public static void LogFatal<T>(T message, [CallerMemberName] string caller = null, [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.LogFatal($"Fatal from {caller} method at line {sourceLineNumber}");
            logger.LogFatal(message);
        }

        public static void LogDebug<T>(T message, [CallerMemberName] string caller = null, [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.LogDebug($"Debug from {caller} method at line {sourceLineNumber}");
            logger.LogDebug(message);
        }

        public static void LogException(Exception ex, [CallerMemberName] string caller = null, [CallerLineNumber] int sourceLineNumber = 0)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Exception from {caller} method at line {sourceLineNumber}");
            builder.AppendLine($"Type: {ex.GetType().FullName}");
            builder.AppendLine($"Message: {ex.Message}");
            builder.AppendLine($"StackTrace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                builder.AppendLine("InnerException:");
                builder.AppendLine($"Type: {ex.InnerException.GetType().FullName}");
                builder.AppendLine($"Message: {ex.InnerException.Message}");
                builder.AppendLine($"StackTrace: {ex.InnerException.StackTrace}");
            }

            logger.LogError(builder.ToString());
        }
    }
}
