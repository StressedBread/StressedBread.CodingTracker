using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.Model;
using CodingTracker.StressedBread.UI;
using System.Diagnostics;

namespace CodingTracker.StressedBread;

internal class StopWatchSession
{
    MainHelpers mainHelpers = new();
    CodingController codingController = new();
    public Stopwatch stopwatch = new();
    RecordUI recordUI = new();

    internal void StartSession()
    {
        stopwatch.Reset();
        stopwatch.Start();

        DateTime startTime = DateTime.Now;

        bool isRunning = true, saveSession;

        saveSession = recordUI.StartCodingSessionDisplay(this, ref isRunning);
        if (saveSession)
        {
            SaveSession(startTime);
        }

        stopwatch.Stop();
    }
    internal void SaveSession(DateTime startTime)
    {
        DateTime endTime = DateTime.Now;
        string startFormattedTime, endFormattedTime;
        int durationOut;

        (startFormattedTime, endFormattedTime, durationOut) =  mainHelpers.GetInputAndDuration(startTime, endTime);

        CodingSession codingSession = new(startFormattedTime, endFormattedTime, durationOut);
        codingController.AddRecordQuery(codingSession);
    }
    internal string GetFormattedElapsedTime()
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        return elapsed.ToString(@"hh\:mm\:ss");
    }

}
