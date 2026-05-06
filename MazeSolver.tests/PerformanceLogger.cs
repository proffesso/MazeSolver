using System;
using System.Diagnostics;
using NUnit.Framework;

namespace MazeSolver.tests
{
    internal static class PerformanceLogger
    {
        public static T Measure<T>(string scenario, string algorithmName, Func<T> action)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = action();
            stopwatch.Stop();
            TestContext.WriteLine($"{scenario} - {algorithmName}: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");
            return result;
        }
    }
}