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
                    INSERT INTO Movie (Title, Duration, MinimunAge, MovieCast)
                    VALUES (@title, @duration, @minimumage, @moviecast);";

                command.Parameters.AddWithValue("@title", movie.Title);
                command.Parameters.AddWithValue("@duration", movie.Duration);
                command.Parameters.AddWithValue("@minimumage", movie.Duration);
                command.Parameters.AddWithValue("@moviecast", movie.MovieCast);

                command.ExecuteNonQuery();
                return true;
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

        public List<string> GetAllMovies()
        {
            var movies = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Title, Duration, MinimunAge FROM Movie;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movies.Add($"ID: {reader.GetInt32(0)}, Title: {reader.GetString(1)}, Duration: {reader.GetString(2)}, Minimum age: {reader.GetString(3)}");
                    }
                }
            }
            return movies;
        }
    }
}
