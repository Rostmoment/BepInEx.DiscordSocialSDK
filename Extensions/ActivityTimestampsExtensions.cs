using System;

namespace BepInEx.DiscordSocialSDK.RPC
{
    public static class ActivityTimestampsExtensions
    {
        // Reference point for Unix time (1970-01-01 00:00:00 UTC)
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a <see cref="DateTime"/> to milliseconds since the Unix epoch.
        /// Local and unspecified <see cref="DateTimeKind"/> values are automatically
        /// converted to UTC before the calculation.
        /// </summary>
        /// <param name="dt">The date and time to convert.</param>
        /// <returns>Milliseconds elapsed since 1970-01-01 00:00:00 UTC.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="dt"/> is earlier than the Unix epoch.
        /// </exception>
        private static ulong ToUnixMilliseconds(DateTime dt)
        {
            DateTime utc = dt.Kind == DateTimeKind.Utc
                ? dt
                : dt.ToUniversalTime();

            double ms = (utc - UnixEpoch).TotalMilliseconds;

            if (ms < 0)
                throw new ArgumentOutOfRangeException(nameof(dt), "DateTime must be after Unix epoch (1970-01-01 UTC).");

            return (ulong)ms;
        }

        /// <summary>
        /// Sets the activity start time from a <see cref="DateTime"/> value.
        /// Discord will display a count-up timer showing how long the user
        /// has been in this activity.
        /// </summary>
        /// <param name="timestamps">The timestamps instance to modify</param>
        /// <param name="start">The start time of the activity</param>
        public static void SetStart(this ActivityTimestamps timestamps, DateTime start)
        {
            timestamps.SetStart(ToUnixMilliseconds(start));
        }

        /// <summary>
        /// Sets the activity end time from a <see cref="DateTime"/> value.
        /// Discord will display a count-down timer showing how much time remains
        /// </summary>
        /// <param name="timestamps">The timestamps instance to modify</param>
        /// <param name="end">The expected end time of the activity</param>
        public static void SetEnd(this ActivityTimestamps timestamps, DateTime end)
        {
            timestamps.SetEnd(ToUnixMilliseconds(end));
        }

        /// <summary>
        /// Sets the activity start time to the current UTC time.
        /// A shorthand for <c>SetStart(DateTime.UtcNow)</c> when the activity
        /// begins at the moment of calling
        /// </summary>
        /// <param name="timestamps">The timestamps instance to modify</param>
        public static void SetStartNow(this ActivityTimestamps timestamps)
        {
            timestamps.SetStart(DateTime.UtcNow);
        }
    }
}