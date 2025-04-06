using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class MovieRepository
    {
        public void AddMovie(string title, int duration, int minimunAge)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Movie (Title, Duration, MinimumAge)
                    VALUES (@title, @duration, @minimumAge);";

                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@duration", duration);
                command.Parameters.AddWithValue("@minimumAge", minimunAge);
                // command.Parameters.AddWithValue("@movieCast", movieCast);

                command.ExecuteNonQuery();
                Console.WriteLine("Movie added successfully.");
            }
        }

        public bool DeleteMovie(Movie movie)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM Movie WHERE Title = @Title";

                command.Parameters.AddWithValue("@Title",movie.Title);
                
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
                command.CommandText = "SELECT Id, Title, Duration, MinimumAge FROM Movie;";

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
