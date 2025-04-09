using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class LocationRepository
    {
        public bool AddLocation(Location location)
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

        public bool RemoveLocation(Location location)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Location> GetAllLocations()
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
        public bool ModifyLocation(Location location)
        {
            return true;
        }
    }
}
