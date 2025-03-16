namespace CodingTracker.StressedBread.Model;

/// <summary>
/// Represents the data of the weekly coding statistics from the database.
/// </summary>

internal class WeeklyGoalStatistics
{
    public TimeSpan Goal { get; set; }
    public TimeSpan TimeLeft { get; set; }
    public TimeSpan TimeCoded { get; set; }

    public WeeklyGoalStatistics(double goal, double timeLeft, double timeCoded) 
    {
        Goal = TimeSpan.FromHours(goal);
        TimeLeft = TimeSpan.FromSeconds(timeLeft);
        TimeCoded = TimeSpan.FromSeconds(timeCoded);
    }
}
