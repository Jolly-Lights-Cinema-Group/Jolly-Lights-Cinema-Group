using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class DatabaseManager
    {
        private static readonly string _dbPath = "Jolly-Lights-Cinema-Group\\Database\\cinema.db";
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
                command.CommandText = File.ReadAllText("Jolly-Lights-Cinema-Group\\Database\\schema.sql");
                command.ExecuteNonQuery();
                Console.WriteLine("Database initialized.");
            }
        }
    }
}
