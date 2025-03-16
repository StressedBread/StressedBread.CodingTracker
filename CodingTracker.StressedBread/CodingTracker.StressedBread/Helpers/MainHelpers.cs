using CodingTracker.StressedBread.Model;
using Spectre.Console;
using System.Configuration;
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
        string? dbPath = ConfigurationManager.AppSettings["DBPath"];
        string path;
        if (string.IsNullOrEmpty(dbPath))
        {
            AnsiConsole.MarkupLine("[red bold]Database folder path not in config file! Setting the path manually.[/]");
            Thread.Sleep(2000);
            path = Path.Combine(Directory.GetCurrentDirectory(), "Database");
        }
        else
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), dbPath);
        }            

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
