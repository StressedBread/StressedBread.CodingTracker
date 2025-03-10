using Spectre.Console;
using System.Globalization;

namespace CodingTracker.StressedBread.Helpers;

internal class Validation
{
    public DateTime DateTimeValidation(string time)
    {
        string[] formattedDateTime =
        {
            "dd/MM/yyyy HH:mm",
            "d/M/yyyy HH:mm",
            "dd/M/yyyy HH:mm",
            "d/MM/yyyy HH:mm",
            "dd/MM/yyyy H:mm",
            "d/M/yyyy H:mm",
            "dd/M/yyyy H:mm",
            "d/MM/yyyy H:mm",

            "dd/MM/yy HH:mm",
            "d/M/yy HH:mm",
            "dd/M/yy HH:mm",
            "d/MM/yy HH:mm",
            "dd/MM/yy H:mm",
            "d/M/yy H:mm",
            "dd/M/yy H:mm",
            "d/MM/yy H:mm"
        };

        while (string.IsNullOrEmpty(time) || !DateTime.TryParseExact(time, formattedDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Please try again.[/]");
            time = AnsiConsole.Ask<string>("");
        }

        DateTime date = DateTime.ParseExact(time, formattedDateTime, CultureInfo.InvariantCulture);

        return date;

    }
    public bool DurationValidation(int duration)
    {
        return duration >= 0;
    }
}
