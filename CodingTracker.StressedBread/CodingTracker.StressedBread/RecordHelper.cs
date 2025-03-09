using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Model;
using Spectre.Console;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread;
internal class RecordHelper
{
    CodingController codingController = new();
    Helpers helpers = new();
    Validation validation = new();
    internal void ViewRecordsHelper()
    {
        var records = codingController.ViewRecords();

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
                helpers.FormattedDuration(record.Duration)
                );
        }

        AnsiConsole.Write(table);
        Console.ReadKey();
    }
    internal void AddRecordHelper()
    {
        DateTime startTimeFormatted = helpers.GetInput("start");
        DateTime endTimeFormatted = helpers.GetInput("end");

        int duration = helpers.DurationCalculation(startTimeFormatted, endTimeFormatted);

        if (!validation.DurationValidation(duration))
        {
            validation.ShowInvalidMessage();
            AddRecordHelper();
        }
        else
        {
            CodingSession newSession = new(startTimeFormatted.ToString(), endTimeFormatted.ToString(), duration);
            codingController.AddRecord(newSession);
        }
    }
    internal void EditRecordHelper()
    {
        var records = codingController.ViewRecords();

        AnsiConsole.MarkupLine("[darkorange bold]Select a record to edit[/]\n");

        AnsiConsole.MarkupLine("  | Start Time          | End Time            | Duration |");
        AnsiConsole.MarkupLine("  |---------------------|---------------------|----------|");

        var recordToEdit = AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
            .PageSize(20)
            .UseConverter(r => {

                return $"| {r.StartTime,-18} | {r.EndTime,-18} | {helpers.FormattedDuration(r.Duration),-8} |";
            })
            .AddChoices(records));

        Console.Clear();

        var editChoice = AnsiConsole.Prompt(new SelectionPrompt<EditChoice>()
            .Title("Select what to edit")
            .AddChoices(Enum.GetValues<EditChoice>()));

        switch (editChoice)
        {
            case EditChoice.StartTime:
                EditStartTime(recordToEdit);
                break;

            case EditChoice.EndTime:
                EditEndTime(recordToEdit);
                break;

            case EditChoice.Both:
                EditBoth(recordToEdit);
                break;
        }
    }
    internal void DeleteRecordHelper()
    {
        var records = codingController.ViewRecords();        

        AnsiConsole.MarkupLine("[red bold]Select a record to delete[/]\n");

        AnsiConsole.MarkupLine("  | Start Time          | End Time            | Duration |");
        AnsiConsole.MarkupLine("  |---------------------|---------------------|----------|");

        var recordToDelete = AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
            .PageSize(20)
            .UseConverter(r => {

                return $"| {r.StartTime,-18} | {r.EndTime,-18} | {helpers.FormattedDuration(r.Duration),-8} |";
                })
            .AddChoices(records));

        codingController.DeleteRecord(recordToDelete.Id);
    }
    private void EditStartTime(CodingSession recordToEdit)
    {
        DateTime newStartTimeFormatted = helpers.GetInput("start");

        int durationStart = helpers.DurationCalculation(newStartTimeFormatted, DateTime.Parse(recordToEdit.EndTime));

        if (!validation.DurationValidation(durationStart))
        {
            validation.ShowInvalidMessage();
            EditRecordHelper();
        }
        else
        {
            UpdateRecord(recordToEdit.Id, newStartTimeFormatted.ToString(), recordToEdit.EndTime.ToString(), durationStart, EditChoice.StartTime);
        }
    }
    private void EditEndTime(CodingSession recordToEdit)
    {
        DateTime newEndTimeFormatted = helpers.GetInput("end");

        int durationEnd = helpers.DurationCalculation(DateTime.Parse(recordToEdit.StartTime), newEndTimeFormatted);

        if (!validation.DurationValidation(durationEnd))
        {
            validation.ShowInvalidMessage();
            EditRecordHelper();
        }
        else
        {
            UpdateRecord(recordToEdit.Id, recordToEdit.StartTime, newEndTimeFormatted.ToString(), durationEnd, EditChoice.EndTime);
        }
    }
    private void EditBoth(CodingSession recordToEdit)
    {
        DateTime newStartTimeBothFormatted = helpers.GetInput("start");
        DateTime newEndTimeBothFormatted = helpers.GetInput("end");

        int durationBoth = helpers.DurationCalculation(newStartTimeBothFormatted, newEndTimeBothFormatted);

        if (!validation.DurationValidation(durationBoth))
        {
            validation.ShowInvalidMessage();
            EditRecordHelper();
        }
        else
        {
            UpdateRecord(recordToEdit.Id, newStartTimeBothFormatted.ToString(), newEndTimeBothFormatted.ToString(), durationBoth, EditChoice.Both);
        }
    }
    private void UpdateRecord(long id, string startTime, string endTime, int duration, EditChoice choice)
    {
        CodingSession codingSession = new(id, startTime, endTime, duration, choice);
        codingController.EditRecord(codingSession);
    }
}
