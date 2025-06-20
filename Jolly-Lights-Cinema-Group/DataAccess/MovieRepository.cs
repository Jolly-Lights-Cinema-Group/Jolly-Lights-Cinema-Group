using Jolly_Lights_Cinema_Group.Helpers;
using Microsoft.Data.Sqlite;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class MovieRepository
    {
        public bool AddMovie(Movie movie)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Movie (Title, Duration, MinimunAge,ReleaseDate, MovieCast)
                    VALUES (@title, @duration, @minimumage, @releasedate, @moviecast);";

                command.Parameters.AddWithValue("@title", movie.Title);
                command.Parameters.AddWithValue("@duration", movie.Duration);
                command.Parameters.AddWithValue("@minimumage", movie.MinimumAge);
                command.Parameters.AddWithValue("@releasedate", movie.ReleaseDate);
                command.Parameters.AddWithValue("@moviecast", movie.MovieCast);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public virtual Movie? GetMovieById(int index)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, Title, Duration, MinimunAge, ReleaseDate, MovieCast FROM Movie
                WHERE Id = @Id;";

                command.Parameters.AddWithValue("@Id", index);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Movie(
                            reader.GetInt32(0), // id
                            reader.GetString(1), // Title
                            reader.GetInt32(2),  // Duration
                            reader.GetInt32(3),  // MinimunAge
                            reader.GetDateTime(4), // ReleaseDate
                            reader.GetString(5)  // MovieCast
                        );
                    }
                }
            }
            return null;
        }

        public Movie? GetMovieByTitle(string Title)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, Title, Duration, MinimunAge, ReleaseDate, MovieCast FROM Movie
                WHERE Title = @Title;";

                command.Parameters.AddWithValue("@Title", Title);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Movie(
                            reader.GetInt32(0), // id
                            reader.GetString(1), // Title
                            reader.GetInt32(2),  // Duration
                            reader.GetInt32(3),  // MinimunAge
                            reader.GetDateTime(4), // ReleaseDate
                            reader.GetString(5)  // MovieCast
                        );
                    }
                }
            }
            return null;
        }


        public List<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, Title, Duration, MinimunAge, ReleaseDate, MovieCast FROM Movie;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // This is a small filter function to check if the Column is null or not.
                        // Reader will throw errors if the column value is null.
                        string SafeGet(int index) => reader.IsDBNull(index) ? "" : reader.GetString(index);

                        movies.Add(new Movie(
                            reader.GetInt32(0),
                            SafeGet(1),
                            reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            reader.GetDateTime(4),
                            SafeGet(5)
                        ));
                    }
                }
            }
            return movies;
        }

        public Movie? ModifyMovie(Movie movie, string? newTitle, string? newDuration, string? newMinimumAge, string? newReleaseDate, string? newCast)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();

                var updates = new List<string>();
                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    movie.Title = newTitle;
                    updates.Add("Title = @newTitle");
                    parameters["@newTitle"] = newTitle;
                }

                if (!string.IsNullOrWhiteSpace(newDuration))
                {
                    if (int.TryParse(newDuration, out int validDuration))
                    {
                        movie.Duration = validDuration;
                        updates.Add("Duration = @newDuration");
                        parameters["@newDuration"] = validDuration;
                    }
                }

                if (!string.IsNullOrWhiteSpace(newMinimumAge))
                {
                    if (int.TryParse(newMinimumAge, out int validMinimumAge))
                    {
                        movie.MinimumAge = validMinimumAge;
                        updates.Add("MinimumAge = @newMinimumAge");
                        parameters["@newMinimumAge"] = validMinimumAge;
                    }
                }

                if (!string.IsNullOrWhiteSpace(newReleaseDate))
                {
                    if (DateTimeValidator.TryParseDate(newReleaseDate, out  DateTime validReleaseDate))
                    {
                        movie.ReleaseDate = validReleaseDate;
                        updates.Add("ReleaseDate = @newReleaseDate");
                        parameters["@newReleaseDate"] = validReleaseDate;
                    }
                }

                if (!string.IsNullOrWhiteSpace(newCast))
                {
                    movie.MovieCast = newCast;
                    updates.Add("MovieCast = @newCast");
                    parameters["@newCast"] = newCast;
                }

                if (updates.Count == 0)
                {
                    return null;
                }

                var command = connection.CreateCommand();
                command.CommandText = $@"
                    UPDATE Movie
                    SET {string.Join(", ", updates)}
                    WHERE Id = @id;";

                command.Parameters.AddWithValue("@id", movie.Id);

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                return movie;
            }
        }
    }
}

