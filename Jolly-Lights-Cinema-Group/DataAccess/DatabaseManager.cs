using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class DatabaseManager
    {
        private static readonly string _dbPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Database", "cinema.db");
        private static readonly string _connectionString = $"Data Source={_dbPath}";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public static void InitializeDatabase()
        {
            if (!File.Exists(_dbPath))
            {
                Console.WriteLine("Database not found. Creating new database...");
                Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
                File.Create(_dbPath).Close();
                CreateTables();
            }
        }

        private static void CreateTables()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                string schemaPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Database", "schema.sql");
                command.CommandText = File.ReadAllText(schemaPath);
                command.ExecuteNonQuery();
                Console.WriteLine("Database initialized.");
            }
        }
    }
}
