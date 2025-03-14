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
        AscendingType ascendingTypeOut;
        bool isAscending;
        string? startTemp = "", endTemp = "", startLabel = "", endLabel = "";
        DateTime startTime = DateTime.MinValue;
        DateTime endTime = DateTime.MinValue;

        (filterPeriodOut, filterTypeOut) = recordUI.GetFilterChoice();

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
                startTemp = mainHelpers.FormattedDateTimeFilter(startTime, filterPeriodOut);
            }
            else
            {
                startTemp = validation.ValidateWeekAndYear(startTemp);
                startTemp = mainHelpers.FormattedDateTimeFilter(startTime, filterPeriodOut, startTemp);
            }
        }
        if (requiresEnd) 
        {
            endTemp = recordUI.GetInput($"({endLabel})", filterPeriodOut);
            if (filterPeriodOut != FilterPeriod.Week)
            {
                endTime = validation.DateTimeValidation(endTemp, filterPeriodOut);
                endTemp = mainHelpers.FormattedDateTimeFilter(endTime, filterPeriodOut);
            }
            else
            {
                endTemp = validation.ValidateWeekAndYear(endTemp);
                endTemp = mainHelpers.FormattedDateTimeFilter(endTime, filterPeriodOut,endTemp);
            }
        } 

        var records = codingController.FilteredRecordsQuery(filterPeriodOut, filterTypeOut, startTemp, endTemp);

        if (recordUI.DisplayData(records, true, false))
        {
            ascendingTypeOut = recordUI.FilterOrder();
            isAscending = recordUI.IsAscending();
            records = codingController.FilteredRecordsQuery(filterPeriodOut, filterTypeOut, startTemp, endTemp, isAscending, ascendingTypeOut);
            recordUI.DisplayData(records, true, true);
        }

        Console.ReadKey();
    }
    internal void ReportHelper()
    {
        var sumRecords = codingController.SumDurationQuery();
        var avgRecords = codingController.AvgDurationQuery();
        int weeklyGoal = codingController.ViewGoal();

        TimeSpan sum = TimeSpan.FromSeconds(sumRecords.sumDurationOut);
        TimeSpan sumLastWeek = TimeSpan.FromSeconds(sumRecords.lastWeekDuration);
        TimeSpan sumLastYear = TimeSpan.FromSeconds(sumRecords.lastYearDuration);

        TimeSpan avg = TimeSpan.FromSeconds(avgRecords.avgDurationOut);
        TimeSpan avgLastWeek = TimeSpan.FromSeconds(avgRecords.lastWeekDuration);
        TimeSpan avgLastYear = TimeSpan.FromSeconds(avgRecords.lastYearDuration);

        TimeSpan weeklyGoalSpan = TimeSpan.FromHours(weeklyGoal);

        recordUI.DisplayReport(sum, sumLastWeek, sumLastYear, avg, avgLastWeek, avgLastYear, weeklyGoalSpan);
        Console.ReadKey();
    }
    internal void GoalHelper()
    {
        int weeklyGoal = recordUI.GetWeeklyGoal();
        codingController.UpdateGoalQuery(weeklyGoal);
    }
}
