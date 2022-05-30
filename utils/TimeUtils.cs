using System.Diagnostics;

namespace Pong.utils
{
    static class TimeUtils
    {
        public static long GetNanoSeconds()
        {
            double timestamp = Stopwatch.GetTimestamp();
            return (long)(1000000000.0 * timestamp / Stopwatch.Frequency);
        }
    }
}
