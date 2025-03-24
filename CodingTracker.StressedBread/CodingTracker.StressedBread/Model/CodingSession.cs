using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Model;

/// <summary>
/// Represents data of the coding session from database.
/// </summary>

internal class CodingSession
{
    public long Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public long Duration { get; set; }
    public EditChoice Choice { get; set; }

    public CodingSession()
    {
        StartTime = string.Empty;
        EndTime = string.Empty;
    }

    public CodingSession(string startTime, string endTime, int duration)
    {
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }

    public CodingSession(long id, string startTime, string endTime, long duration)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }
}
