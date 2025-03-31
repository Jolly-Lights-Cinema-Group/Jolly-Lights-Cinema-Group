using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleRepository
    {
        public void AddSchedule(int movieRoomId, int movieId, DateTime startDate)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Schedule (MovieRoomId, MovieId, StartDate)
                    VALUES (@movieRoomId, @movieId, @startDate);";

                command.Parameters.AddWithValue("@movieRoomId", movieRoomId);
                command.Parameters.AddWithValue("@movieId", movieId);
                command.Parameters.AddWithValue("@startDate", startDate);

                command.ExecuteNonQuery();
                Console.WriteLine("Schedule added successfully.");
            }
        }
    }
}
