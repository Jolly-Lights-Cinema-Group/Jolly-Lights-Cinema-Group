using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class MovieRoomRepository
    {
        public void AddMovieRoom(int roomNumber, string roomLayoutJson, int supportedMovieType, int locationId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO MovieRoom (RoomNumber, RoomLayoutJson, SupportedMovieType, LocationId)
                    VALUES (@roomNumber, @roomLayoutJson, @supportedMovieType, @locationId);";

                command.Parameters.AddWithValue("@roomNumber", roomNumber);
                command.Parameters.AddWithValue("@roomLayoutJson", roomLayoutJson);
                command.Parameters.AddWithValue("@supportedMovieType", supportedMovieType);
                command.Parameters.AddWithValue("@locationId", locationId);

                command.ExecuteNonQuery();
                Console.WriteLine("Movie room added successfully.");
            }
        }

        public List<string> GetAllMovieRooms(int locationId)
        {
            var movieRooms = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, RoomNumber, SupportedMovieType FROM MovieRoom WHERE LocationId = @LocationId;";
                command.Parameters.AddWithValue("@LocationId", locationId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movieRooms.Add($"ID: {reader.GetInt32(0)}, Room number: {reader.GetString(1)}, Supported movie type: {reader.GetString(2)}");
                    }
                }
            }
            return movieRooms;
        }
    }
}
