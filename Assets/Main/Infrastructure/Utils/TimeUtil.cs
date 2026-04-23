using System;
using UnityEditor;

namespace Main.Infrastructure.Utils
{
    public static class TimeUtil
    {
        private const int _TARGET_FRAME = 60;
        
        public static DateTime ToDateTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(timestamp);
        }
        
        public static long ToTimeStamp(this DateTime dateTime)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) (dateTime - epochStart).TotalMilliseconds;
        }
        
        public static int ToDays(this DateTime dateTime)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (dateTime - epochStart).Days;
        }
        
        public static DateTime MsToDateTime(long ms)
        {
            return DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).AddMilliseconds(ms);
        }

        public static float FramesToSeconds(float frames)
        {
            return frames / _TARGET_FRAME;
        }
        
        public static float SecondsToFrames(float frames)
        {
            return frames * _TARGET_FRAME;
        }
        
        public static bool HasNegativeValue(long time)
        {
            return CurrentTimeInMs() - time < 0;
        }
        
        public static int ConvertMsToMinutes(long milliseconds)
        {
            return (int) TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
        }

        public static int ConvertMinToSec(int min)
        {
            return min * 60;
        }
        
        public static int ConvertMinToMs(int min)
        {
            return min * 60 * 1000;
        }

        public static long ConvertDayToMs(int days)
        {
            return (long)days * 24 * 60 * 60 * 1000L;
        }
        
        public static long ConvertHourToMs(int hours)
        {
            return (long)hours * 60 * 60 * 1000L;
        }
        
        public static long CurrentTimeInMs()
        {
            #if UNITY_EDITOR
            var minutes = EditorPrefs.GetInt("minutes");
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + minutes * 60 * 1000;
            #else
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            #endif
        }
    }
}