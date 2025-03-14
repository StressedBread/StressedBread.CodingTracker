using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.Model;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.UI;

internal class RecordUI
{
    CodingController codingController = new();
    MainHelpers mainHelpers = new();

    internal bool DisplayData(List<CodingSession> records, bool filtered, bool order = false)
    {
        Console.Clear();
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
                mainHelpers.FormattedDuration(record.Duration)
                );
        }

        AnsiConsole.Write(table);

        if (!filtered)
            return ConfirmationPrompt("Do you wish to filter?");
        if (order)
            return ConfirmationPrompt("Do you wish to order the results?");

        return false;
    }
    internal CodingSession RecordToSelect(string op)
    {
        var records = codingController.ViewRecordsQuery();
        string format = "dd/MM/yyyy HH:mm:ss";

        AnsiConsole.MarkupLine($"[darkorange bold]Select a record to {op}.[/]\n");

        AnsiConsole.MarkupLine("  | Start Time          | End Time            | Duration    |");
        AnsiConsole.MarkupLine("  |---------------------|---------------------|-------------|");

        return AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
            .PageSize(20)
            .UseConverter(r =>
            {

                return $"| {DateTime.Parse(r.StartTime).ToString(format),-18} " +
                        $"| {DateTime.Parse(r.EndTime).ToString(format),-18} " +
                        $"| {mainHelpers.FormattedDuration(r.Duration),-11} |";
            })
            .AddChoices(records));
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
    internal void ShowInvalidDurationMessage()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[red]Invalid input. Duration cannot be negative. Press any key to return to menu[/]");
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
    internal (FilterPeriod filterPeriodOut, FilterType filterTypeOut) GetFilterChoice()
    {
        return (
            AnsiConsole.Prompt(new SelectionPrompt<FilterPeriod>()
            .Title("\nSelect the period to filter")
            .AddChoices(Enum.GetValues<FilterPeriod>())),

            AnsiConsole.Prompt(new SelectionPrompt<FilterType>()
            .Title("\nSelect the type to filter")
            .AddChoices(Enum.GetValues<FilterType>()))
            );
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
}
