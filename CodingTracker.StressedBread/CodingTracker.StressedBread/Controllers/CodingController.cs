using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.StressedBread.Controllers;

internal class CodingController
{
    DatabaseController databaseController = new();
    internal void CreateTableOnStart()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS CodingTracker (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTIme TEXT,
                Duration INTEGER
            )";
        databaseController.Execute(query);
    }

    internal void AddRecord(DateTime startTime, DateTime endTime, int duration)
    {
        var query = @"
            INSERT INTO CodingTracker (StartTime, EndTime, Duration)
            VALUES (@StartTime, @EndTime, @Duration)";
        
        DynamicParameters parameters = new();
        
        parameters.Add("@StartTime", startTime);
        parameters.Add("@EndTime", endTime);
        parameters.Add("@Duration", duration);

        databaseController.Execute(query, parameters);
    }

    internal int DurationCalculation(DateTime startTime, DateTime endTime)
    {
        int duration = (int)(endTime - startTime).TotalMinutes;
        return duration;
    }
}
