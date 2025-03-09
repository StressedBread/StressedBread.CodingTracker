using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.Model;
using Spectre.Console;
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
            table.AddRow(
                record.Id.ToString(),
                record.StartTime.Substring(0, 10),
                record.StartTime.Substring(11, 5),
                record.EndTime.Substring(0, 10),
                record.EndTime.Substring(11, 5),
                mainHelpers.FormattedDuration(record.Duration)
                );
        }

        AnsiConsole.Write(table);
    }
    internal CodingSession RecordToSelect(string op)
    {
        var records = codingController.ViewRecords();

        AnsiConsole.MarkupLine($"[darkorange bold]Select a record to {op}.[/]\n");

        AnsiConsole.MarkupLine("  | Start Time          | End Time            | Duration |");
        AnsiConsole.MarkupLine("  |---------------------|---------------------|----------|");

        return AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
            .PageSize(20)
            .UseConverter(r => {

                return $"| {r.StartTime,-18} | {r.EndTime,-18} | {mainHelpers.FormattedDuration(r.Duration),-8} |";
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
}
