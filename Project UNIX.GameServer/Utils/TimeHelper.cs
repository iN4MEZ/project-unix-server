using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUNIX.GameServer.Utils
{
    public static class TimeHelper
    {
        public static DateTime UnixSecondsToDateTime(uint timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }
        public static DateTime UnixMillisecondsToDateTime(uint timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
        }
    }
}
