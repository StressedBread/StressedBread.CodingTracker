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

        DateTime startDateTime = validation.DateTimeValidation(recordUI.GetInput("start"));
        DateTime endDateTime = validation.DateTimeValidation(recordUI.GetInput("end"));

        (startDateTimeOut, endDateTimeOut, durationOut) = mainHelpers.GetInputAndDuration(startDateTime, endDateTime);

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
        string startDateTimeOut, endDateTimeOut, startTemp, endTemp;
        int durationOut;

        var recordToEdit = recordUI.RecordToSelect("edit");

        Console.Clear();

        var editChoice = recordUI.GetEditChoice();

        DateTime newStartDateTime = DateTime.Parse(recordToEdit.StartTime);
        DateTime newEndDateTime = DateTime.Parse(recordToEdit.EndTime);

        switch (editChoice)
        {
            case EditChoice.StartTime:
                startTemp = recordUI.GetInput("start");
                newStartDateTime = validation.DateTimeValidation(startTemp);
                break;

            case EditChoice.EndTime:
                endTemp = recordUI.GetInput("end");
                newEndDateTime = validation.DateTimeValidation(endTemp);
                break;

            case EditChoice.Both:
                startTemp = recordUI.GetInput("start");
                endTemp = recordUI.GetInput("end");
                newStartDateTime = validation.DateTimeValidation(startTemp);
                newEndDateTime = validation.DateTimeValidation(endTemp);
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
        FilterPeriod filterPeriodOut;
        FilterType filterTypeOut;
        string? startTemp = "", endTemp = "", startLabel = "", endLabel = "";
        DateTime startTime, endTime;

        (filterPeriodOut, filterTypeOut) = recordUI.GetFilterChoice();

        switch (filterPeriodOut)
        {
            case FilterPeriod.Day:
                startLabel = "start";
                endLabel = "end";
                break;
            case FilterPeriod.Week:
                startLabel = "Week(s)/Year";
                endLabel = "Week(s)/Year";
                break;
            case FilterPeriod.Month:
                startLabel = "Month(s)/Year";
                endLabel = "Month(s)/Year";
                break;
            case FilterPeriod.Year:
                startLabel = "Year";
                endLabel = "Year";
                break;
        }

        bool requiresStart = filterTypeOut == FilterType.AllAfterIncluding || filterTypeOut == FilterType.AllBetweenIncluding;
        bool requiresEnd = filterTypeOut == FilterType.AllBeforeIncluding || filterTypeOut == FilterType.AllBetweenIncluding;

        if (requiresStart) 
        {
            startTemp = recordUI.GetInput($"({startLabel})", filterPeriodOut);
            startTime = validation.DateTimeValidation(startTemp, filterPeriodOut);
            startTemp = mainHelpers.FormattedDateTime(startTime);
        }
        if (requiresEnd) 
        {
            endTemp = recordUI.GetInput($"({endLabel})", filterPeriodOut);
            endTime = validation.DateTimeValidation(endTemp, filterPeriodOut);
            endTemp = mainHelpers.FormattedDateTime(endTime);
        } 

        codingController.FilteredRecordsQuery(filterPeriodOut, filterTypeOut, startTemp, endTemp);

        /*switch ((filterPeriodOut, filterTypeOut))
        {
            case (FilterPeriod.Day, FilterType.AllAfterIncluding):
                startTemp = recordUI.GetInput("start");                
                break;
            case (FilterPeriod.Day, FilterType.AllBeforeIncluding):
                endTemp = recordUI.GetInput("end");
                break;
            case (FilterPeriod.Day, FilterType.AllBetweenIncluding):
                startTemp = recordUI.GetInput("start");
                endTemp = recordUI.GetInput("end");
                break;
        }*/

        Console.ReadKey();
    }
}
