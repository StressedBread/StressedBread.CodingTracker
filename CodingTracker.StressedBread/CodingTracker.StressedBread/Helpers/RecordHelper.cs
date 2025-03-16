using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Model;
using CodingTracker.StressedBread.UI;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Helpers;

/// <summary>
///  The RecordHelper class provides methods for interacting with coding session records, including viewing, adding, editing, deleting, filtering, generating reports, and updating goals. 
/// </summary>

internal class RecordHelper
{
    CodingController codingController = new();
    MainHelpers mainHelpers = new();
    Validation validation = new();
    RecordUI recordUI = new();
    StringFormatting stringFormatting = new();

    internal void ViewRecordsHelper()
    {
        var records = codingController.ViewRecordsQuery();
        if (recordUI.DisplayData(records, false)) FilterRecordsHelper();
    }
    internal void AddRecordHelper()
    {
        DateTime startDateTime = validation.DateTimeValidation(recordUI.GetInput("start"));
        DateTime endDateTime = validation.DateTimeValidation(recordUI.GetInput("end"));

        var inputAndDur = stringFormatting.GetInputAndDuration(startDateTime, endDateTime);

        if (!validation.DurationValidation(inputAndDur.Duration))
        {
            recordUI.ShowInvalidDurationMessage();
            return;
        }

        CodingSession newSession = new(inputAndDur.StartDateTime.ToString(), inputAndDur.EndDateTime.ToString(), inputAndDur.Duration);
        codingController.AddRecordQuery(newSession);
        codingController.GoalDurationQuery();
    }
    internal void EditRecordHelper()
    {
        string startTemp, endTemp;

        var recordToEdit = recordUI.RecordToSelect("edit");
        var recordToDisplay = new List<CodingSession>{ recordToEdit };

        Console.Clear();

        recordUI.DisplayData(recordToDisplay, true);
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

       var inputAndDur = stringFormatting.GetInputAndDuration(newStartDateTime, newEndDateTime, recordToEdit, editChoice);

        if (!validation.DurationValidation(inputAndDur.Duration))
        {
            recordUI.ShowInvalidDurationMessage();
            return;
        }

        CodingSession codingSession = new(recordToEdit.Id, inputAndDur.StartDateTime, inputAndDur.EndDateTime, inputAndDur.Duration);
        codingController.EditRecordQuery(codingSession);
        codingController.GoalDurationQuery();

        recordUI.ShowSuccessMessage("updated");
    }
    internal void DeleteRecordHelper()
    {        
        var recordToDelete = recordUI.RecordToSelect("delete");
        Console.Clear();
        if (recordUI.ConfirmationPrompt("Are you sure?"))
        {
            codingController.DeleteRecordQuery(recordToDelete.Id);
            codingController.GoalDurationQuery();
            recordUI.ShowSuccessMessage("deleted");
        }

    }
    internal void FilterRecordsHelper()
    {        
        AscendingType ascendingTypeOut;
        bool isAscending;
        string? startTemp = "", endTemp = "", startLabel = "", endLabel = "";
        DateTime startTime = DateTime.MinValue;
        DateTime endTime = DateTime.MinValue;

        var filterChoice = recordUI.GetFilterChoice();
        FilterPeriod filterPeriodOut = filterChoice.FilterPeriod;
        FilterType filterTypeOut = filterChoice.FilterType;

        switch (filterPeriodOut)
        {
            case FilterPeriod.Day:
                startLabel = "start";
                endLabel = "end";
                break;
            case FilterPeriod.Week:
                startLabel = "Start Week(s)/Year";
                endLabel = "End Week(s)/Year";
                break;
            case FilterPeriod.Month:
                startLabel = "Start Month(s)/Year";
                endLabel = "End Month(s)/Year";
                break;
            case FilterPeriod.Year:
                startLabel = "Start Year";
                endLabel = "End Year";
                break;
        }

        bool requiresStart = filterTypeOut == FilterType.AllAfterIncluding || filterTypeOut == FilterType.AllBetweenIncluding;
        bool requiresEnd = filterTypeOut == FilterType.AllBeforeIncluding || filterTypeOut == FilterType.AllBetweenIncluding;

        if (requiresStart) 
        {
            startTemp = recordUI.GetInput($"({startLabel})", filterPeriodOut);
            if (filterPeriodOut != FilterPeriod.Week)
            {
                startTime = validation.DateTimeValidation(startTemp, filterPeriodOut);
                startTemp = stringFormatting.FormattedDateTimeFilter(startTime, filterPeriodOut);
            }
            else
            {
                startTemp = validation.ValidateWeekAndYear(startTemp);
                startTemp = stringFormatting.FormattedDateTimeFilter(startTime, filterPeriodOut, startTemp);
            }
        }
        if (requiresEnd) 
        {
            endTemp = recordUI.GetInput($"({endLabel})", filterPeriodOut);
            if (filterPeriodOut != FilterPeriod.Week)
            {
                endTime = validation.DateTimeValidation(endTemp, filterPeriodOut);
                endTemp = stringFormatting.FormattedDateTimeFilter(endTime, filterPeriodOut);
            }
            else
            {
                endTemp = validation.ValidateWeekAndYear(endTemp);
                endTemp = stringFormatting.FormattedDateTimeFilter(endTime, filterPeriodOut,endTemp);
            }
        } 

        var records = codingController.FilteredRecordsQuery(filterPeriodOut, filterTypeOut, startTemp, endTemp);

        if (recordUI.DisplayData(records, true, false))
        {
            ascendingTypeOut = recordUI.FilterOrder();
            isAscending = recordUI.IsAscending();
            records = codingController.FilteredRecordsQuery(filterPeriodOut, filterTypeOut, startTemp, endTemp, isAscending, ascendingTypeOut);
            recordUI.DisplayData(records, true, true);
            Console.ReadKey();
        }
    }
    internal void ReportHelper()
    {
        var sumRecords = codingController.SumDurationQuery();
        var avgRecords = codingController.AvgDurationQuery();
        var weeklyGoal = codingController.ViewGoalQuery();
        var daysToMonday = codingController.GetDaysLeftToMonday();

        var timePerDay =  mainHelpers.CalculateCodingPerDay(daysToMonday, weeklyGoal.TimeLeft);

        recordUI.DisplayReport(sumRecords, avgRecords, weeklyGoal, timePerDay);
        Console.ReadKey();
    }
    internal void GoalHelper()
    {
        int weeklyGoal = recordUI.GetWeeklyGoal();
        codingController.UpdateGoalQuery(weeklyGoal);
    }
}
