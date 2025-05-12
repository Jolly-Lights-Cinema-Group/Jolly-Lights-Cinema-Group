using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleRepository
    {
        public bool AddSchedule(Schedule schedule)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Schedule (MovieRoomId, MovieId, StartDate, StartTime)
                    VALUES (@movieRoomId, @movieId, @startDate,@startTime);";

                command.Parameters.AddWithValue("@movieRoomId", schedule.MovieRoomId);
                command.Parameters.AddWithValue("@movieId", schedule.MovieId);
                command.Parameters.AddWithValue("@startDate", schedule.StartDate);
                command.Parameters.AddWithValue("@startTime", schedule.StartTime);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public void UpdateFreeTimeColumn()
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Schedule
                    SET FreeAfter = time(StartTime, '+' || (SELECT Duration + 15 FROM Movie WHERE Id = Schedule.MovieId) || ' minutes')
                    WHERE EXISTS (SELECT 1 FROM Movie WHERE Id = Schedule.MovieId);";

                command.ExecuteNonQuery();
            }
        }

        public bool CanAddSchedule(int roomId, DateTime startDate, TimeSpan startTime)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM Schedule
                    WHERE MovieRoomId = @roomId
                    AND StartDate = @startDate
                    AND @startTime < FreeAfter";

                command.Parameters.AddWithValue("@roomId", roomId);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@startTime", startTime);

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count == 0;
            }
        }

        public int GetMovieDuration(int movieId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Duration FROM Movie WHERE Id = @movieId;";
                command.Parameters.AddWithValue("@movieId", movieId);

                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }


}
