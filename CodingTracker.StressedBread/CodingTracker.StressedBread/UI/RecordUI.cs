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

    internal void DisplayData(List<CodingSession> records)
    {
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
        Console.Clear();
        bool confirmation = AnsiConsole.Prompt(new ConfirmationPrompt(text));
        return confirmation;
    }
    internal bool StartCodingSessionDisplay(StopWatchManager stopWatchManager, ref bool isRunning)
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

            AnsiConsole.Markup($"Elapsed time:[grey50] {stopWatchManager.GetFormattedElapsedTime()}[/]");
            Thread.Sleep(1000);
        }
        return false;
    }
}
