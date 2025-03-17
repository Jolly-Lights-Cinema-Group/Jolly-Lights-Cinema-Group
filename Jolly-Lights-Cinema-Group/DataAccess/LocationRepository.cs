using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class LocationRepository
    {
        public void AddLocation(string name, string address)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Location (Name, Address)
                    VALUES (@name, @address);";

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@address", address);

                command.ExecuteNonQuery();
                Console.WriteLine("Location added successfully.");
            }
        }

        public List<string> GetAllLocations()
        {
            var locations = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Address FROM Loaction;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locations.Add($"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)}, Address: {reader.GetString(2)}");
                    }
                }
            }
            return locations;
        }
    }
}
