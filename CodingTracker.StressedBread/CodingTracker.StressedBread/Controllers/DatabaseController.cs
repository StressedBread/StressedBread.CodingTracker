using Microsoft.Data.Sqlite;
using System.Configuration;
using Dapper;

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
}
