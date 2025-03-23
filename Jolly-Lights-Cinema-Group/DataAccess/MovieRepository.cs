using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class MovieRepository
    {
        public void AddMovie(string title, int duration, int minimunAge, string movieCast)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Movie (Title, Duration, MinimumAge, MovieCast)
                    VALUES (@title, @duration, @minimumAge, @movieCast);";

                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@duration", duration);
                command.Parameters.AddWithValue("@minimumAge", minimunAge);
                command.Parameters.AddWithValue("@movieCast", movieCast);

                command.ExecuteNonQuery();
                Console.WriteLine("Movie added successfully.");
            }
        }

        public List<string> GetAllMovies()
        {
            var movies = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Title, Duration, MinimumAge, MovieCast FROM Movie;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movies.Add($"ID: {reader.GetInt32(0)}, Title: {reader.GetString(1)}, Duration: {reader.GetString(2)}, Minimum age: {reader.GetString(3)}, Movie cast: {reader.GetString(4)}");
                    }
                }
            }
            return movies;
        }
    }
}
