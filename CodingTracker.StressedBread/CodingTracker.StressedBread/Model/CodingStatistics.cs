namespace CodingTracker.StressedBread.Model;

internal class CodingStatistics
{
    public TimeSpan TotalDuration { get; set; }
    public TimeSpan LastWeekDuration { get; set; }
    public TimeSpan LastYearDuration { get; set; }

    public CodingStatistics(double totalDuration, double lastWeekDuration, double lastYearDuration)
    {
        TotalDuration = TimeSpan.FromSeconds(totalDuration);
        LastWeekDuration = TimeSpan.FromSeconds(lastWeekDuration);
        LastYearDuration = TimeSpan.FromSeconds(lastYearDuration);
    }
}
