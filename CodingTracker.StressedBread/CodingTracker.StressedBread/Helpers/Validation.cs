using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;

/// <summary>
/// Handles the date and duration validation.
/// </summary>

internal class Validation
{
    internal DateTime DateTimeValidation(string time, FilterPeriod? filterPeriod = null)
    {
        string[] format;
        string[] formattedDateTime =
        {
            "dd/MM/yyyy HH:mm:ss",
            "d/M/yyyy HH:mm:ss",
            "dd/M/yyyy HH:mm:ss",
            "d/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy H:mm:ss",
            "d/M/yyyy H:mm:ss",
            "dd/M/yyyy H:mm:ss",
            "d/MM/yyyy H:mm:ss"
        };
        string[] formattedFilterDay =
        {
            "dd/MM/yyyy",
            "d/M/yyyy",
            "dd/M/yyyy",
            "d/MM/yyyy",
            "dd/MM/yyyy",
            "d/M/yyyy",
            "dd/M/yyyy",
            "d/MM/yyyy"
        };

        bool isWeek = false;

        DateTime date;
        
        switch (filterPeriod)
        {
            case FilterPeriod.Day:
                format = formattedFilterDay;
                break;
            case FilterPeriod.Month:
                format = formattedFilterDay;
                time = $"01/{time}";
                break;
            case FilterPeriod.Year:
                format = formattedFilterDay;
                time = $"01/01/{time}";
                break;
            default:
                format = formattedDateTime;
                break;
        }
        if (!isWeek)
        {
            while (string.IsNullOrEmpty(time) || !DateTime.TryParseExact(time, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please try again.[/]");
                time = AnsiConsole.Ask<string>("");
                switch (filterPeriod)
                {
                    case FilterPeriod.Month:
                        time = $"01/{time}";
                        break;
                    case FilterPeriod.Year:
                        time = $"01/01/{time}";
                        break;
                }
            }            
        }
        return date = DateTime.ParseExact(time, format, CultureInfo.InvariantCulture);
    }
    internal string ValidateWeekAndYear(string input)
    {
        string weekPattern = @"^(0[0-9]|[1-4][0-9]|5[0-3])/\b((19|20)\d{2})\b";

        Match match = Regex.Match(input, weekPattern);

        while (string.IsNullOrEmpty(input) || !match.Success)
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Please try again.[/]");
            input = AnsiConsole.Ask<string>("");
            match = Regex.Match(input, weekPattern);
        }      

        return input; 
    }
    internal bool DurationValidation(int duration)
    {
        return duration >= 0;
    }
}
