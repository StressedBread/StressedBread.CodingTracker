using CodingTracker.StressedBread.Model;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;

/// <summary>
/// Provides methods for duration calculation, database folder creation and closing application.
/// </summary>

class MainHelpers
{
    internal void CloseApplication()
    {
        Environment.Exit(0);
    }
    internal int DurationCalculation(DateTime startTime, DateTime endTime)
    {
        double duration = (endTime - startTime).TotalSeconds;
        return (int)Math.Round(duration);
    }    
    internal TimeSpan CalculateCodingPerDay(double daysToMonday, TimeSpan timeLeft)
    {
        TimeSpan timeToMonday = TimeSpan.FromDays(daysToMonday);
        double result = timeLeft/timeToMonday;
        return TimeSpan.FromDays(result);
    }
    internal void CreateDatabaseFolder()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Database");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
