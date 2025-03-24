namespace CodingTracker.StressedBread;

/// <summary>
/// Stores all the enum values.
/// </summary>

internal class Enums
{
    internal enum MenuChoice
    {
        ViewRecords,
        AddRecord,
        EditRecord,
        DeleteRecord,
        CodingSession,
        Report,
        SetGoal,
        CloseApplication
    };

    internal enum EditChoice
    {
        StartTime,
        EndTime,
        Both
    };

    internal enum FilterPeriod
    {
        Day,
        Week,
        Month,
        Year
    }

    internal enum FilterType
    {
        AllAfterIncluding,
        AllBeforeIncluding,
        AllBetweenIncluding
    };

    internal enum AscendingType
    {
        Id,
        StartTime,
        EndTime,
        Duration
    };
}
