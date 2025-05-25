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
                    VALUES (@movieRoomId, @movieId, @startDate, @startTime);";

                command.Parameters.AddWithValue("@movieRoomId", schedule.MovieRoomId);
                command.Parameters.AddWithValue("@movieId", schedule.MovieId);
                command.Parameters.AddWithValue("@startDate", schedule.StartDate);
                command.Parameters.AddWithValue("@startTime", schedule.StartTime);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateFreeTimeColumn()
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Schedule
                    SET FreeAfter = time(StartTime, '+' || (SELECT Duration + 15 FROM Movie WHERE Id = Schedule.MovieId) || ' minutes')
                    WHERE EXISTS (SELECT 1 FROM Movie WHERE Id = Schedule.MovieId);";

                return command.ExecuteNonQuery() > 0;
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

        // Not needed anymore
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

        public bool DeleteScheduleLine(Schedule schedule)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                                        DELETE FROM Schedule 
                                        WHERE Id = @id ;";

                command.Parameters.AddWithValue("@id", schedule.Id);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<Schedule> ShowSchedule(DateTime date)
        {
            List<Schedule> schedules = new List<Schedule>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id, MovieRoomId, MovieId, StartDate, StartTime FROM Schedule WHERE DATE(StartDate) = @startDate;";

                command.Parameters.Add(new SqliteParameter("@startDate", System.Data.DbType.String) { Value = date.ToString("yyyy-MM-dd") });

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Schedule schedule = new(
                            reader.GetInt32(0),     // Id
                            reader.GetInt32(1),     // Roomid
                            reader.GetInt32(2),     // MovieId
                            reader.GetDateTime(3),  // StartDate
                            reader.GetTimeSpan(4)); //StartTime
                        schedules.Add(schedule);
                    }
                }
            }
            return schedules;
        }

        public List<Schedule> GetAllSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id, MovieRoomId, MovieId, StartDate, StartTime FROM Schedule;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Schedule schedule = new(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.GetTimeSpan(4));
                        schedules.Add(schedule);
                    }
                }
            }

            return schedules;
        }

        public List<Schedule> GetSchedulesByMovie(Movie movie)
        {
            List<Schedule> schedules = new List<Schedule>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id, MovieRoomId, MovieId, StartDate, StartTime FROM Schedule WHERE MovieId = @movieId;";

                command.Parameters.AddWithValue("@movieId", movie.Id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Schedule schedule = new(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.GetTimeSpan(4));
                        schedules.Add(schedule);
                    }
                }
            }
            return schedules;
        }

        public List<Schedule> GetScheduleByMovieAndRoom(Movie movie, MovieRoom movieRoom)
        {
            List<Schedule> schedules = new List<Schedule>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id, MovieRoomId, MovieId, StartDate, StartTime FROM Schedule WHERE MovieId = @movieId AND MovieRoomId = @movieRoomId;";

                command.Parameters.AddWithValue("@movieId", movie.Id);
                command.Parameters.AddWithValue("@movieRoomId", movieRoom.Id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Schedule schedule = new(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.GetTimeSpan(4));
                        schedules.Add(schedule);
                    }
                }
            }
            return schedules;
        }
    }
}
