using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.Model;
using CodingTracker.StressedBread.UI;

namespace CodingTracker.StressedBread;

internal class StopWatchSession
{
    RecordUI recordUI = new();
    StopWatchManager stopWatchManager = new();
    MainHelpers mainHelpers = new();
    CodingController codingController = new();

    internal void StartSession()
    {
        stopWatchManager.Reset();
        stopWatchManager.Start();

        DateTime startTime = DateTime.Now;

        bool isRunning = true;
        bool saveSession;

        saveSession = recordUI.StartCodingSessionDisplay(stopWatchManager, ref isRunning);
        if (saveSession)
        {
            SaveSession(startTime);
        }

        stopWatchManager.Stop();
    }

    internal void SaveSession(DateTime startTime)
    {
        DateTime endTime = DateTime.Now;

        string endFormattedTime = mainHelpers.FormattedDateTime(endTime);
        string startFormattedTime = mainHelpers.FormattedDateTime(startTime);

        int duration = mainHelpers.DurationCalculation(startTime, endTime);

        CodingSession codingSession = new(startFormattedTime, endFormattedTime, duration);
        codingController.AddRecordQuery(codingSession);
    }
}
