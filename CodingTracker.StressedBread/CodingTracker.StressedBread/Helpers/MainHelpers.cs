using CodingTracker.StressedBread.Model;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;

class MainHelpers
{
    Validation validation = new();

    internal DateTime GetInput(string inputType)
    {
        AnsiConsole.MarkupLine($"Enter the new [darkorange]{inputType}[/] time of the coding session in the format:\n[darkorange bold]Day(s)/Month(s)/Year Hour(s):Minute(s) (24-hour format)[/]");
        string newTime = AnsiConsole.Ask<string>("");
        return validation.DateTimeValidation(newTime);
    }
    internal void CloseApplication()
    {
        Environment.Exit(0);
    }
    internal int DurationCalculation(DateTime startTime, DateTime endTime)
    {
        double duration = (endTime - startTime).TotalSeconds;
        return (int)Math.Round(duration);
    }
    internal string FormattedDuration(long duration)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        int totalHours = (int)timeSpan.TotalHours;
        return $"{totalHours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
    }
    internal string FormattedDateTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
    internal (string startDateTimeOut, string endDateTimeOut, int durationOut) GetInputAndDuration(DateTime startDateTime, DateTime endDateTime)
    {
       return ProcessDateTime(startDateTime, endDateTime);
    }
    internal (string startDateTimeOut, string endDateTimeOut, int durationOut) GetInputAndDuration(DateTime startDateTime, DateTime endDateTime, CodingSession recordToEdit, EditChoice editChoice)
    {
        string startFormattedTime = recordToEdit.StartTime;
        string endFormattedTime = recordToEdit.EndTime;

        switch (editChoice)
        {
            case EditChoice.StartTime:
                startFormattedTime = FormattedDateTime(startDateTime);
                break;

            case EditChoice.EndTime:
                endFormattedTime = FormattedDateTime(endDateTime);
                break;

            case EditChoice.Both:
                return ProcessDateTime(startDateTime, endDateTime);
        }

        int duration = DurationCalculation(startDateTime, endDateTime);

        return (startFormattedTime, endFormattedTime, duration);
    }
    internal (string startDateTimeOut, string endDateTimeOut, int durationOut) ProcessDateTime(DateTime startDateTime, DateTime endDateTime)
    {
        string startFormattedTime = FormattedDateTime(startDateTime);
        string endFormattedTime = FormattedDateTime(endDateTime);

        int duration = DurationCalculation(startDateTime, endDateTime);

        return (startFormattedTime, endFormattedTime, duration);
    }
}
