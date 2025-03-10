using System.Diagnostics;

namespace CodingTracker.StressedBread;

internal class StopWatchManager
{
    public Stopwatch stopwatch = new();

    internal void Start()
    {
        stopwatch.Start();
    }
    internal void Stop()
    {
        stopwatch.Stop();
    }
    internal void Reset()
    {
        stopwatch.Reset();
    }
    internal string GetFormattedElapsedTime()
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        return elapsed.ToString(@"hh\:mm\:ss");
    }
}
