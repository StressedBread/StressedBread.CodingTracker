using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.Model;
using CodingTracker.StressedBread.UI;
using System.Diagnostics;

namespace CodingTracker.StressedBread;

/// <summary>
/// Handles live coding session using a StopWatch.
/// </summary>

internal class StopWatchSession
{
    MainHelpers mainHelpers = new();
    CodingController codingController = new();
    public Stopwatch stopwatch = new();
    RecordUI recordUI = new();
    StringFormatting stringFormatting = new();

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

        var inputAndDur = stringFormatting.GetInputAndDuration(startTime, endTime);

        CodingSession codingSession = new(inputAndDur.StartDateTime, inputAndDur.EndDateTime, inputAndDur.Duration);
        codingController.AddRecordQuery(codingSession);
        codingController.GoalDurationQuery();
    }
    internal string GetFormattedElapsedTime()
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        return elapsed.ToString(@"hh\:mm\:ss");
    }
}
