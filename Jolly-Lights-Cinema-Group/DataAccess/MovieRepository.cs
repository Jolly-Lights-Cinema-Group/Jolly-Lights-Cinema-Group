using System;
using System.Collections.Generic;
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

        public bool DeleteMovie(Movie movie)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM Movie WHERE Title = @Title";

                command.Parameters.AddWithValue("@Title", movie.Title);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public Movie? GetMovieByTitle(Movie movie)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Title, Duration, MinimunAge, ReleaseDate, MovieCast FROM Movie
                WHERE Title = @Title;";

                command.Parameters.AddWithValue("@Title", movie.Title);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Movie(
                            reader.GetString(0), // Title
                            reader.GetInt32(1),  // Duration
                            reader.GetInt32(2),  // MinimunAge
                            reader.GetDateTime(3), // ReleaseDate
                            reader.GetString(4)  // MovieCast
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
                SELECT Title, Duration, MinimunAge, ReleaseDate, MovieCast FROM Movie;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // This is a small filter function to check if the Column is null or not.
                        // Reader will throw errors if the column value is null.
                        string SafeGet(int index) => reader.IsDBNull(index) ? "" : reader.GetString(index);

                        movies.Add(new Movie(
                            SafeGet(0),
                            reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            reader.GetDateTime(3),
                            SafeGet(4)
                        ));
                    }
                }
            }
            return movies;
        }
    }
}

