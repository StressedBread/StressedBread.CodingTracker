using CodingTracker.StressedBread.Model;
using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;

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
    internal string FormattedDateTimeFilter(DateTime dateTime, FilterPeriod filterPeriod, string week = "")
    {
        switch (filterPeriod)
        {
            case FilterPeriod.Day:
                return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            case FilterPeriod.Week:
                string[] parts = week.Split('/');
                return $"{parts[1]}-{parts[0]}";;
            case FilterPeriod.Month:
                return dateTime.ToString("yyyy-MM", CultureInfo.InvariantCulture);
            case FilterPeriod.Year:
                return dateTime.ToString("yyyy", CultureInfo.InvariantCulture);
        }

        return "Invalid Input!";        
    }
    internal (string startDateTimeOut, string endDateTimeOut, int durationOut) GetInputAndDuration(DateTime startDateTime, DateTime endDateTime)
    {
        string startFormattedTime = FormattedDateTime(startDateTime);
        string endFormattedTime = FormattedDateTime(endDateTime);

        int duration = DurationCalculation(startDateTime, endDateTime);

        return (startFormattedTime, endFormattedTime, duration);
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
                return GetInputAndDuration(startDateTime, endDateTime);
        }

        int duration = DurationCalculation(startDateTime, endDateTime);

        return (startFormattedTime, endFormattedTime, duration);
    }
}
