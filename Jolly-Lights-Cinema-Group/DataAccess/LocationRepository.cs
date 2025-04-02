using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class LocationRepository
    {
        public void AddLocation(Location location)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Location (Name, Address)
                    VALUES (@name, @address);";

                command.Parameters.AddWithValue("@name", location.Name);
                command.Parameters.AddWithValue("@address", location.Address);

                command.ExecuteNonQuery();
                Console.WriteLine("Location added successfully.");
            }
        }

        public void RemoveLocation(Location location)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM Location
                    WHERE Name = @name AND Address = @address;";

                command.Parameters.AddWithValue("@name", location.Name);
                command.Parameters.AddWithValue("@address", location.Address);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Location removed successfully.");
                }
                else
                {
                    Console.WriteLine("No matching location found to remove.");
                }
            }
        }

        public List<string> GetAllLocations()
        {
            var locations = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Address FROM Location;";

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
