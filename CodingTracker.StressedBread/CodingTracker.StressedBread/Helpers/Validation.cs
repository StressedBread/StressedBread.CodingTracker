using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;

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
        string[] formattedFilterMonth =
        {
            "MM/yyyy",
            "M/yyyy"
        };
        string[] formattedFilterYear =
        {
            "yyyy"
        };

        bool isWeek = false;

        DateTime date;
        
        //I need to output the checked input as string and I need to implement proper checking and formatting of week filter

        switch (filterPeriod)
        {
            case FilterPeriod.Day:
                format = formattedFilterDay;
                break;
            case FilterPeriod.Week:
                isWeek = true;
                format = [];
                break;
            case FilterPeriod.Month:
                format = formattedFilterMonth;
                time = $"01/{time}";
                break;
            case FilterPeriod.Year:
                format = formattedFilterYear;
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
            }
        }
        return date = DateTime.ParseExact(time, format, CultureInfo.InvariantCulture);

    }
    internal void ValidateWeekAndYear(string input)
    {
        string weekPattern = @"^([1-9]|[1-4][0-9]|5[0-3])/\b((19|20)\d{2})\b";

        Match match = Regex.Match(input, weekPattern);

        while (string.IsNullOrEmpty(input) || !match.Success)
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Please try again.[/]");
            input = AnsiConsole.Ask<string>("");
        }
    }
    internal bool DurationValidation(int duration)
    {
        return duration >= 0;
    }
}
