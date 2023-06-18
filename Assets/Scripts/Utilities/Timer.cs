using System;
using System.Diagnostics;

namespace Utils
{
    public static class Timer
    {
        /// <summary>
        /// Use this to quickly create a stopwatch before a function is called
        /// Works in tandem with the "LogStopWatch" utility
        /// </summary>
        /// <returns></returns>
        public static Stopwatch StartTimer()
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            return stopWatch;
        }

        /// <summary>
        /// Place this at the end of a function or section to get a log of how long the function took
        /// </summary>
        /// <param name="timer"></param>
        public static string StopTimer(Stopwatch timer)
        {
            timer.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = timer.Elapsed;

            // Format and display the TimeSpan value.
            return $"{ts.Seconds:00}.{ts.Milliseconds:000}";
        }

        /// <summary>
        /// Get the current time of the timer
        /// </summary>
        /// <param name="timer"></param>
        public static string GetTime(Stopwatch timer)
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = timer.Elapsed;

            // Format and display the TimeSpan value.
            return $"{ts.Seconds:00}.{ts.Milliseconds:000}";
        }
    }
}