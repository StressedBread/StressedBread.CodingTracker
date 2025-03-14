using CodingTracker.StressedBread.Model;
using Dapper;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Controllers;

internal class CodingController
{
    DatabaseController databaseController = new();
    internal void CreateTableOnStartQuery()
    {
        var queryCodingTable = @"
            CREATE TABLE IF NOT EXISTS CodingTracker (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration INTEGER
            )";

        var queryGoalTable = @"CREATE TABLE IF NOT EXISTS CodingGoal (
                                WeeklyCodingGoal INTEGER UNIQUE,
                                LeftCodingTime STRING UNIQUE)";

        var queryTrigger = @"CREATE TRIGGER EnforceSingleRow
                            BEFORE INSERT ON CodingGoal
                            WHEN (SELECT COUNT(*) FROM CodingGoal) >= 1
                            BEGIN
                                SELECT RAISE(FAIL, 'Only one row allowed in CodingGoal');
                            END;";

        var queryTriggerExists = @"SELECT name FROM sqlite_master WHERE type='trigger' AND name='EnforceSingleRow'";
        var queryInsertFirstRow = @"INSERT INTO CodingGoal (WeeklyCodingGoal)
                                    SELECT 0 WHERE NOT EXISTS (SELECT 1 FROM CodingGoal)";

        var triggerExists = databaseController.TriggerExists(queryTriggerExists);       

        
        databaseController.Execute(queryCodingTable);
        databaseController.Execute(queryGoalTable);

        databaseController.Execute(queryInsertFirstRow);
        if (string.IsNullOrEmpty(triggerExists)) databaseController.Execute(queryTrigger);
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
    internal (int sumDurationOut, int lastWeekDuration, int lastYearDuration) SumDurationQuery()
    {
        string sumDurationQuery = @"SELECT SUM(Duration) FROM CodingTracker";

        string lastWeekDurationQuery = @"SELECT SUM(Duration) FROM CodingTracker 
                                    WHERE StartTime 
                                    BETWEEN datetime('now', '-6 days') AND datetime('now', 'localtime')";

        string lastYearDurationQuery = @"SELECT SUM(Duration) FROM CodingTracker 
                                    WHERE StartTime 
                                    BETWEEN datetime('now', 'start of year') AND datetime('now', 'localtime')";

        int sumDuration = databaseController.SumDurationReader(sumDurationQuery);
        int lastWeekDuration = databaseController.SumDurationReader(lastWeekDurationQuery);
        int lastYearDuration = databaseController.SumDurationReader(lastYearDurationQuery);

        return (sumDuration, lastWeekDuration, lastYearDuration);
    }
    internal (double avgDurationOut, double lastWeekDuration, double lastYearDuration) AvgDurationQuery()
    {
        string avgDurationQuery = @"SELECT AVG(Duration) FROM CodingTracker";

        string lastWeekDurationQuery = @"SELECT AVG(Duration) FROM CodingTracker 
                                    WHERE StartTime 
                                    BETWEEN datetime('now', '-6 days') AND datetime('now', 'localtime')";

        string lastYearDurationQuery = @"SELECT AVG(Duration) FROM CodingTracker 
                                    WHERE StartTime 
                                    BETWEEN datetime('now', 'start of year') AND datetime('now', 'localtime')";

        double avgDuration = databaseController.AvgDurationReader(avgDurationQuery);
        double lastWeekDuration = databaseController.AvgDurationReader(lastWeekDurationQuery);
        double lastYearDuration = databaseController.AvgDurationReader(lastYearDurationQuery);

        return (avgDuration, lastWeekDuration, lastYearDuration);
    }
    internal void UpdateGoalQuery(int hours)
    {
        DynamicParameters parameters = new();
        var query = @"UPDATE CodingGoal 
                      SET WeeklyCodingGoal = @weeklyGoal
                      WHERE ROWID = 1";
        parameters.Add("@weeklyGoal", hours);

        databaseController.Execute(query);
    }
    internal int ViewGoal()
    {
        var query = @"Select WeeklyCodingGoal FROM CodingGoal";
        return databaseController.WeeklyGoalReader(query);
    }
}
