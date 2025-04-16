using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public static class LocationRepository
    {
        public static bool AddLocation(Location location)
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
                return true;
            }
        }

        public static bool RemoveLocation(Location location)
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

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static List<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Name, Address FROM Location;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Location location = new(reader.GetString(0), reader.GetString(1));
                        locations.Add(location);
                    }
                }
            }
            return locations;
        }
        public static bool ModifyLocation(Location oldLocation, string? newName, string? newAddress)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();

                var updates = new List<string>();
                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    updates.Add("Name = @newName");
                    parameters["@newName"] = newName;
                }

                if (!string.IsNullOrWhiteSpace(newAddress))
                {
                    updates.Add("Address = @newAddress");
                    parameters["@newAddress"] = newAddress;
                }

                if (updates.Count == 0)
                {
                    return false;
                }

                var command = connection.CreateCommand();
                command.CommandText = $@"
                    UPDATE Location
                    SET {string.Join(", ", updates)}
                    WHERE Name = @oldName AND Address = @oldAddress;";

                command.Parameters.AddWithValue("@oldName", oldLocation.Name);
                command.Parameters.AddWithValue("@oldAddress", oldLocation.Address);

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
