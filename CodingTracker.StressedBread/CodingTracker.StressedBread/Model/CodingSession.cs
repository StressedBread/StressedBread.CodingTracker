namespace CodingTracker.StressedBread.Model;

internal class CodingSession
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; }

    public CodingSession(DateTime startTime, DateTime endTime, int duration)
    {
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }
}
