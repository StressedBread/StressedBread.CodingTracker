using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.Model;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.UI;

/// <summary>
/// Handles all the UI for displaying data, reports and goal. Also handles the UI side of user input. 
/// </summary>

internal class RecordUI
{
    CodingController codingController = new();
    StringFormatting stringFormatting = new();

    internal bool DisplayData(List<CodingSession> records, bool filtered, bool notOrder = true)
    {
        Console.Clear();

        if (records.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found. Press any key to return to menu[/]");
            Console.ReadKey();
            return false;
        }

        Table table = new();

        table.AddColumn("Id");
        table.AddColumn("Start Date");
        table.AddColumn("Start Time");
        table.AddColumn("End Date");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach (var column in table.Columns)
        {
            column.Centered();
        }

        foreach (var record in records)
        {
            DateTime startTime = DateTime.Parse(record.StartTime);
            DateTime endTime = DateTime.Parse(record.EndTime);
            string formattedStartTime = startTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string formattedEndTime = endTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            table.AddRow(
                record.Id.ToString(),
                formattedStartTime.Substring(0, 10),
                formattedStartTime.Substring(11, 8),
                formattedEndTime.Substring(0, 10),
                formattedEndTime.Substring(11, 8),
                stringFormatting.FormattedDuration(record.Duration)
                );
        }

        AnsiConsole.Write(table);

        if (!filtered)
            return ConfirmationPrompt("Do you wish to filter?");
        if (!notOrder)
            return ConfirmationPrompt("Do you wish to order the results?");

        return false;
    }

    internal CodingSession? RecordToSelect(string op)
    {
        var records = codingController.ViewRecordsQuery();

        if (records.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found. Press any key to return to menu[/]");
            Console.ReadKey();
            return null;
        }

        string format = "dd/MM/yyyy HH:mm:ss";

        AnsiConsole.MarkupLine($"[darkorange bold]Select a record to {op}.[/]\n");

        AnsiConsole.MarkupLine("  | Start Time          | End Time            | Duration    |");
        AnsiConsole.MarkupLine("  |---------------------|---------------------|-------------|");

        if (records.Count == 1)
        {
            AnsiConsole.MarkupLine($"  | {DateTime.Parse(records[0].StartTime).ToString(format),-18} " +
                $"| {DateTime.Parse(records[0].EndTime).ToString(format),-18} " +
                $"| {stringFormatting.FormattedDuration(records[0].Duration),-11} |");
            AnsiConsole.MarkupLine("\n[darkorange bold]Only one record found. Record selected automatically. Press any key to continue[/]");
            Console.ReadKey();
            return records[0];
        }

        else
        {

            return AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
                .PageSize(20)
                .UseConverter(r =>
                {
                    return $"| {DateTime.Parse(r.StartTime).ToString(format),-18} " +
                            $"| {DateTime.Parse(r.EndTime).ToString(format),-18} " +
                            $"| {stringFormatting.FormattedDuration(r.Duration),-11} |";
                })
                .AddChoices(records));
        }
    }

    internal EditChoice GetEditChoice()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<EditChoice>()
            .Title("Select what to edit")
            .AddChoices(Enum.GetValues<EditChoice>()));
    }

    internal string GetInput(string inputType, FilterPeriod filterPeriod = FilterPeriod.Day)
    {
        switch (filterPeriod)
        {
            case FilterPeriod.Day:
                AnsiConsole.MarkupLine($"Enter the [darkorange]{inputType}[/] date and time of the coding session in the format:" +
                    "\n[darkorange bold]Day(s)/Month(s)/Year Hour(s):Minute(s):Second(s) (24-hour format)[/]\n" +
                    "[red bold]If filtering, exlude time![/]");
                return AnsiConsole.Ask<string>("");

            case FilterPeriod.Week:
                AnsiConsole.MarkupLine($"Enter the [darkorange bold]{inputType}[/] to filter by.");
                return AnsiConsole.Ask<string>("");

            case FilterPeriod.Month:
                AnsiConsole.MarkupLine($"Enter the [darkorange bold]{inputType}[/] to filter by.");
                return AnsiConsole.Ask<string>("");

            case FilterPeriod.Year:
                AnsiConsole.MarkupLine($"Enter the [darkorange bold]{inputType}[/] to filter by.");
                return AnsiConsole.Ask<string>("");

            default:
                return "Invalid";
        }
    }

    internal void ShowInvalidMessage(string message)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[red]Invalid input. {message} cannot be negative. Press any key to return to menu[/]");
        Console.ReadKey();
    }

    internal void ShowSuccessMessage(string op)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[green]Record {op} successfully. Press any key to return to menu[/]");
        Console.ReadKey();
    }

    internal bool ConfirmationPrompt(string text)
    {
        bool confirmation = AnsiConsole.Prompt(new ConfirmationPrompt(text));
        return confirmation;
    }

    internal bool StartCodingSessionDisplay(StopWatchSession stopWatchSession, ref bool isRunning)
    {
        Console.Clear();
        while (isRunning)
        {
            Console.SetCursorPosition(0, 0);
            AnsiConsole.MarkupLine("Press [green bold]Enter[/] to save the session.");
            AnsiConsole.MarkupLine("Press [red bold]Any[/] other key to discard the session.\n");

            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Enter)
                {
                    isRunning = false;
                    return true;
                }
                else
                {
                    isRunning = false;
                    return false;
                }
            }

            AnsiConsole.Markup($"Elapsed time:[grey50] {stopWatchSession.GetFormattedElapsedTime()}[/]");
            Thread.Sleep(1000);
        }

        return false;
    }

    internal FilterChoice GetFilterChoice()
    {
        return new(
            AnsiConsole.Prompt(new SelectionPrompt<FilterPeriod>()
            .Title("\nSelect the period to filter")
            .AddChoices(Enum.GetValues<FilterPeriod>())),

            AnsiConsole.Prompt(new SelectionPrompt<FilterType>()
            .Title("\nSelect the type to filter")
            .AddChoices(Enum.GetValues<FilterType>())));
    }

    internal AscendingType FilterOrder()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<AscendingType>()
            .Title("Choose the column to order by.")
            .AddChoices(Enum.GetValues<AscendingType>()));
    }

    internal bool IsAscending()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title("Choose the ascension to order by.")
            .AddChoices(true, false)
            .UseConverter(x => x switch
            {
                true => "Ascending",
                false => "Descending"
            }));
    }

    internal void DisplayReport(CodingStatistics sumStats, CodingStatistics avgStats, WeeklyGoalStatistics weeklyGoalStats, TimeSpan timePerDay)
    {
        AnsiConsole.MarkupLine($"Your total coding time is: [darkorange bold]{sumStats.TotalDuration.Days} days, {sumStats.TotalDuration.Hours} hours, " +
            $"{sumStats.TotalDuration.Minutes} minutes, {sumStats.TotalDuration.Seconds} seconds[/]");
        AnsiConsole.MarkupLine($"Your coding time this week is: [darkorange bold]{sumStats.LastWeekDuration.Days} days, {sumStats.LastWeekDuration.Hours} hours, " +
            $"{sumStats.LastWeekDuration.Minutes} minutes, {sumStats.LastWeekDuration.Seconds} seconds[/]");
        AnsiConsole.MarkupLine($"Your total coding time this year is: [darkorange bold]{sumStats.ThisYearDuration.Days} days, {sumStats.ThisYearDuration.Hours} hours, " +
            $"{sumStats.ThisYearDuration.Minutes} minutes, {sumStats.ThisYearDuration.Seconds} seconds[/]\n");

        AnsiConsole.MarkupLine($"Your average coding time per session is: [darkorange bold]{avgStats.TotalDuration.Days} days, {avgStats.TotalDuration.Hours} hours, " +
            $"{avgStats.TotalDuration.Minutes} minutes, {avgStats.TotalDuration.Seconds} seconds[/]");
        AnsiConsole.MarkupLine($"Your weekly average coding time is: [darkorange bold]{avgStats.LastWeekDuration.Days} days, {avgStats.LastWeekDuration.Hours} hours, " +
            $"{avgStats.LastWeekDuration.Minutes} minutes, {avgStats.LastWeekDuration.Seconds} seconds[/]");
        AnsiConsole.MarkupLine($"Your average coding time this year is: [darkorange bold]{avgStats.ThisYearDuration.Days} days, {avgStats.ThisYearDuration.Hours} hours, " +
            $"{avgStats.ThisYearDuration.Minutes} minutes, {avgStats.ThisYearDuration.Seconds} seconds[/]\n");

        AnsiConsole.MarkupLine($"Your weekly coding goal: [darkorange bold]{weeklyGoalStats.TimeCoded} / {weeklyGoalStats.Goal}[/]");
        AnsiConsole.MarkupLine($"Time left to code to complete the goal: [darkorange bold]{weeklyGoalStats.TimeLeft}[/]\n");

        AnsiConsole.MarkupLine($"You would have to code [darkorange bold]{timePerDay.Hours}:{timePerDay.Minutes}:{timePerDay.Seconds}[/] per day to reach your goal.");
    }

    internal int GetWeeklyGoal()
    {
        AnsiConsole.MarkupLine($"Enter new weekly coding goal in [darkorange]hours.[/]");
        var result = AnsiConsole.Ask<int>("");
        while (result < 0)
        {
            AnsiConsole.MarkupLine($"[red]Goal can't be negative![/]");
            result = AnsiConsole.Ask<int>("");
        }

        return result;
    }
}
