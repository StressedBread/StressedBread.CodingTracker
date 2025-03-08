﻿using Spectre.Console;

namespace CodingTracker.StressedBread.Helpers;

class MainHelpers
{
    Validation validation = new();

    internal DateTime GetInput(string inputType)
    {
        AnsiConsole.MarkupLine($"Enter the new [darkorange]{inputType}[/] time of the coding session in the format: [darkorange bold]Day(s)/Month(s)/Year Hour(s):Minute(s) (24-hour format)[/]");
        string newTime = AnsiConsole.Ask<string>("");
        return validation.DateTimeValidation(newTime);
    }    
    internal void CloseApplication()
    {
        Environment.Exit(0);
    }
    internal int DurationCalculation(DateTime startTime, DateTime endTime)
    {
        double duration = (endTime - startTime).TotalMinutes;
        return (int)Math.Round(duration);
    }
    internal string FormattedDuration(long duration)
    {
        TimeSpan timeSpan = TimeSpan.FromMinutes(duration);
        int totalHours = (int)timeSpan.TotalHours;
        return $"{totalHours}h {timeSpan.Minutes}m";
    }
}
