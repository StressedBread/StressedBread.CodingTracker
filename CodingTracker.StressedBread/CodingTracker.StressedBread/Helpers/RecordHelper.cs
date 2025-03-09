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
        var records = codingController.ViewRecords();
        recordUI.DisplayData(records);
        Console.ReadKey();
    }
    internal void AddRecordHelper()
    {
        string startDateTimeOut;
        string endDateTimeOut;
        int durationOut;

        (startDateTimeOut, endDateTimeOut, durationOut) = GetAddInputAndDuration();

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
        string startDateTimeOut;
        string endDateTimeOut;
        int durationOut;

        var recordToEdit = recordUI.RecordToSelect("edit");

        Console.Clear();

        var editChoice = recordUI.GetEditChoice();

        (startDateTimeOut, endDateTimeOut, durationOut) = GetEditInputAndDuration(recordToEdit, editChoice);

        if(!validation.DurationValidation(durationOut))
        {
            recordUI.ShowInvalidDurationMessage();
            return;
        }

        switch (editChoice)
        {
            case EditChoice.StartTime:
                UpdateRecord(recordToEdit.Id, startDateTimeOut, recordToEdit.EndTime, durationOut, EditChoice.StartTime);
                break;

            case EditChoice.EndTime:
                UpdateRecord(recordToEdit.Id, recordToEdit.StartTime, endDateTimeOut, durationOut, EditChoice.EndTime);
                break;

            case EditChoice.Both:
                UpdateRecord(recordToEdit.Id, startDateTimeOut, endDateTimeOut, durationOut, EditChoice.Both);
                break;
        }

        recordUI.ShowSuccessMessage("updated");
    }
    internal void DeleteRecordHelper()
    {
        var recordToDelete = recordUI.RecordToSelect("delete");
        if (recordUI.ConfirmationPrompt("Are you sure?"))
        {
            codingController.DeleteRecordQuery(recordToDelete.Id);
            recordUI.ShowSuccessMessage("deleted");
        }
            
    }
    private (string startDateTimeOut, string endDateTimeOut, int durationOut) GetEditInputAndDuration(CodingSession recordToEdit, EditChoice editChoice)
    {
        DateTime startDateTime;
        DateTime endDateTime;
        int duration;
        
        switch (editChoice)
        {
            case EditChoice.StartTime:
                startDateTime = mainHelpers.GetInput("start");
                duration = mainHelpers.DurationCalculation(startDateTime, DateTime.Parse(recordToEdit.EndTime));
                validation.DurationValidation(duration);
                return (startDateTime.ToString(), recordToEdit.EndTime, duration);

            case EditChoice.EndTime:
                endDateTime = mainHelpers.GetInput("end");
                duration = mainHelpers.DurationCalculation(DateTime.Parse(recordToEdit.StartTime), endDateTime);
                validation.DurationValidation(duration);
                return (recordToEdit.StartTime, endDateTime.ToString(), duration);

            case EditChoice.Both:
                startDateTime = mainHelpers.GetInput("start");
                endDateTime = mainHelpers.GetInput("end");
                duration = mainHelpers.DurationCalculation(startDateTime, endDateTime);
                return (startDateTime.ToString(), endDateTime.ToString(), duration);

            default:
                return (recordToEdit.StartTime, recordToEdit.EndTime, (int)recordToEdit.Duration);
        }
    }
    private (string startDateTimeOut, string endDateTimeOut, int durationOut) GetAddInputAndDuration()
    {
        DateTime startDateTime = mainHelpers.GetInput("start");
        DateTime endDateTime = mainHelpers.GetInput("end");
        int duration = mainHelpers.DurationCalculation(startDateTime, endDateTime);
        return (startDateTime.ToString(), endDateTime.ToString(), duration);
    }
    private void UpdateRecord(long id, string startTime, string endTime, int duration, EditChoice choice)
    {
        CodingSession codingSession = new(id, startTime, endTime, duration, choice);
        codingController.EditRecordQuery(codingSession);
    }
}
