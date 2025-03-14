using CodingTracker.StressedBread.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.StressedBread.Controllers;

internal class DatabaseController
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

    internal void Execute(string query, object? parameters = null)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            connection.Execute(query, parameters);
        }
    }
    
    internal List<CodingSession> Reader(string query, object? parameters = null)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            return connection.Query<CodingSession>(query, parameters).ToList();
        }
    }

    internal int SumDurationReader(string query)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            return connection.QuerySingleOrDefault<int?>(query) ?? 0;
        }
    }
    internal double AvgDurationReader(string query)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            return connection.QuerySingleOrDefault<double?>(query) ?? 0;
        }
    }
    internal string? TriggerExists(string query)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            return connection.QuerySingleOrDefault<string?>(query) ?? null;
        }
    }
    internal int WeeklyGoalReader(string query)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            return connection.QuerySingleOrDefault<int?>(query) ?? 0;
        }
    }
}
