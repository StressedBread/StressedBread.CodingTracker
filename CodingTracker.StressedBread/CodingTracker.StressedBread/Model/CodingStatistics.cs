namespace CodingTracker.StressedBread.Model;

/// <summary>
/// Represents data of the coding statistics.
/// </summary>

internal class CodingStatistics
{
    public TimeSpan TotalDuration { get; set; }
    public TimeSpan LastWeekDuration { get; set; }
    public TimeSpan ThisYearDuration { get; set; }

    public CodingStatistics(double totalDuration, double lastWeekDuration, double thisYearDuration)
    {
        TotalDuration = TimeSpan.FromSeconds(totalDuration);
        LastWeekDuration = TimeSpan.FromSeconds(lastWeekDuration);
        ThisYearDuration = TimeSpan.FromSeconds(thisYearDuration);
    }
}
