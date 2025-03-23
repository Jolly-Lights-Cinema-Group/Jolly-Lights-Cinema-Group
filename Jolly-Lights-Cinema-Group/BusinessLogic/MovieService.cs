using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class MovieService
{
    private readonly MovieRepository _movieRepo;

    public MovieService()
    {
        _movieRepo = new MovieRepository();
    }
//   Id INTEGER PRIMARY KEY AUTOINCREMENT,
//   Title TEXT NOT NULL,
//   Duration INTEGER NOT NULL,
//   MinimunAge INTEGER NOT NULL,
//   MovieCast TEXT
    public void RegisterMovie(string title, int duration, int minimumAge, string movieCast)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Error: Title cannot be empty.");
            return;
        }

        _movieRepo.AddMovie(title, duration, minimumAge, movieCast);
    }

    public void ShowAllMovies()
    {
        List<string> movies = _movieRepo.GetAllMovies();
        if (movies.Count == 0)
        {
            Console.WriteLine("No movies found.");
        }
        else
        {
            Console.WriteLine("Movies:");
            foreach (var movie in movies)
            {
                Console.WriteLine(movie);
            }
        }
    }
}
