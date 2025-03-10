using CodingTracker.StressedBread.Model;
using Dapper;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Controllers;

internal class CodingController
{
    DatabaseController databaseController = new();
    internal void CreateTableOnStartQuery()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS CodingTracker (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration INTEGER
            )";
        databaseController.Execute(query);
    }

    internal List<CodingSession> ViewRecordsQuery()
    {
        var query = @"
            SELECT * FROM CodingTracker";
        return databaseController.Reader(query);
    }

    internal void AddRecordQuery(CodingSession session)
    {
        var query = @"
            INSERT INTO CodingTracker (StartTime, EndTime, Duration)
            VALUES (@StartTime, @EndTime, @Duration)";

        DynamicParameters parameters = new();

        parameters.Add("@StartTime", session.StartTime);
        parameters.Add("@EndTime", session.EndTime);
        parameters.Add("@Duration", session.Duration);

        databaseController.Execute(query, parameters);
    }

    internal void EditRecordQuery(CodingSession session)
    {
        DynamicParameters parameters = new();

        var query = @"";
        parameters.Add("@id", session.Id);
        parameters.Add("@duration", session.Duration);

        switch (session.Choice)
        {
            case EditChoice.StartTime:
                query = @"
                        UPDATE CodingTracker 
                        SET StartTime = @startTime, Duration = @duration
                        WHERE Id = @id";
                parameters.Add("@startTime", session.StartTime);
                break;
            case EditChoice.EndTime:
                query = @"
                        UPDATE CodingTracker 
                        SET EndTime = @endTime, Duration = @duration
                        WHERE Id = @id";
                parameters.Add("@endTime", session.EndTime);
                break;
            case EditChoice.Both:
                query = @"
                        UPDATE CodingTracker 
                        SET StartTime = @startTime, EndTime = @endTime, Duration = @duration
                        WHERE Id = @id";
                parameters.Add("@startTime", session.StartTime);
                parameters.Add("@endTime", session.EndTime);
                break;
        }

        databaseController.Execute(query, parameters);
    }
    internal void DeleteRecordQuery(long id)
    {
        var query = @"
            DELETE FROM CodingTracker
            WHERE Id = @id";

        DynamicParameters parameters = new();

        parameters.Add("@id", id);

        databaseController.Execute(query, parameters);
    }

    //TODO: Implement filters
    internal List<CodingSession> FilteredRecordsQuery(FilterPeriod filterPeriod, FilterType filterType, string? startDateTime, string? endDateTime, bool isAscending = true, AscendingType ascendingType = AscendingType.Id)
    {
        DynamicParameters parameters = new();
        string query = "SELECT * FROM CodingTracker WHERE 1=1";

        Dictionary<FilterPeriod, string> filterPeriodDict = new()
        {
            { FilterPeriod.Day, "DATE(StartTime)" },
            { FilterPeriod.Week, "STRFTIME('%Y-%W', StartTime)" },
            { FilterPeriod.Month, "STRFTIME('%Y-%m', StartTime)" },
            { FilterPeriod.Year, "STRFTIME('%Y', StartTime)" }
        };

        if (filterPeriodDict.TryGetValue(filterPeriod, out string? filter))
        {
            switch (filterType)
            {
                case FilterType.AllAfterIncluding:
                    query += $" AND {filter} >= @startDateTime";
                    parameters.Add("@startDateTime", startDateTime);
                    break;
                case FilterType.AllBeforeIncluding:
                    query += $" AND {filter} <= @endDateTime";
                    parameters.Add("@endDateTime", endDateTime);
                    break;
                case FilterType.AllBetweenIncluding:
                    query += $" AND {filter} BETWEEN @startDateTime AND @endDateTime";
                    parameters.Add("@startDateTime", startDateTime);
                    parameters.Add("@endDateTime", endDateTime);
                    break;
            }
        }

        query += isAscending ? $" ORDER BY {ascendingType} ASC" : $" ORDER BY {ascendingType} DESC";

        return databaseController.Reader(query, parameters);
    }
}
