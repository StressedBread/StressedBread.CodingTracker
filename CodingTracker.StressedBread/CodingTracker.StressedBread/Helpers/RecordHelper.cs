using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Model;
using CodingTracker.StressedBread.UI;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;
internal class RecordHelper
{
    CodingController codingController = new();
    MainHelpers mainHelpers = new();
    Validation validation = new();
    RecordUI recordUI = new();

    internal void ViewRecordsHelper()
    {
        var records = codingController.ViewRecordsQuery();
        if (recordUI.DisplayData(records, false)) FilterRecordsHelper();
    }
    internal void AddRecordHelper()
    {
        string startDateTimeOut, endDateTimeOut;
        int durationOut;

        (startDateTimeOut, endDateTimeOut, durationOut) = mainHelpers.GetInputAndDuration(mainHelpers.GetInput("start"), mainHelpers.GetInput("end"));

        if (!validation.DurationValidation(durationOut))
        {
            recordUI.ShowInvalidDurationMessage();
            return;
        }

        CodingSession newSession = new(startDateTimeOut.ToString(), endDateTimeOut.ToString(), durationOut);
        codingController.AddRecordQuery(newSession);
    }
    internal void EditRecordHelper()
    {
        string startDateTimeOut, endDateTimeOut;
        int durationOut;

        var recordToEdit = recordUI.RecordToSelect("edit");

        Console.Clear();

        var editChoice = recordUI.GetEditChoice();

        DateTime newStartDateTime = DateTime.Parse(recordToEdit.StartTime);
        DateTime newEndDateTime = DateTime.Parse(recordToEdit.EndTime);

        switch (editChoice)
        {
            case EditChoice.StartTime:
                newStartDateTime = mainHelpers.GetInput("start");
                break;

            case EditChoice.EndTime:
                newEndDateTime = mainHelpers.GetInput("end");
                break;

            case EditChoice.Both:
                newStartDateTime = mainHelpers.GetInput("start");
                newEndDateTime = mainHelpers.GetInput("end");
                break;
        }

        (startDateTimeOut, endDateTimeOut, durationOut) = mainHelpers.GetInputAndDuration(newStartDateTime, newEndDateTime, recordToEdit, editChoice);

        if (!validation.DurationValidation(durationOut))
        {
            recordUI.ShowInvalidDurationMessage();
            return;
        }

        CodingSession codingSession = new(recordToEdit.Id, startDateTimeOut, endDateTimeOut, durationOut);
        codingController.EditRecordQuery(codingSession);

        recordUI.ShowSuccessMessage("updated");
    }
    internal void DeleteRecordHelper()
    {        
        var recordToDelete = recordUI.RecordToSelect("delete");
        Console.Clear();
        if (recordUI.ConfirmationPrompt("Are you sure?"))
        {
            codingController.DeleteRecordQuery(recordToDelete.Id);
            recordUI.ShowSuccessMessage("deleted");
        }

    }
    internal void FilterRecordsHelper()
    {
        var records = codingController.FilteredRecordsQuery(FilterTypes.Week, 
            mainHelpers.FormattedDateTime(DateTime.Now.AddYears(-1)), 
            mainHelpers.FormattedDateTime(DateTime.Now.AddYears(1)));

        recordUI.DisplayData(records, true);
        Console.ReadKey();
    }
}
