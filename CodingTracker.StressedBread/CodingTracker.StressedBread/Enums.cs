namespace CodingTracker.StressedBread;

internal class Enums
{
    internal enum MenuChoice
    {
        ViewRecords,
        AddRecord,
        EditRecord,
        DeleteRecord,
        CodingSession,
        CloseApplication
    };

    internal enum EditChoice
    {
        StartTime,
        EndTime,
        Both
    };

    internal enum FilterTypes
    {
        Day,
        Week,
        Month,
        Year,
        Ascending,
        Descending
    }
}
