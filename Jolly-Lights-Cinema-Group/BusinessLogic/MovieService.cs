using JollyLightsCinemaGroup.DataAccess;

public class MovieService
{
    private readonly MovieRepository _movieRepo;

    public MovieService()
    {
        _movieRepo = new MovieRepository();
    }
    public void RegisterMovie(Movie movie)
    {
        if (string.IsNullOrWhiteSpace(movie.Title))
        {
            Console.WriteLine("Error: Title cannot be empty.");
            return;
        }

        if (_movieRepo.AddMovie(movie))
        {
            Console.WriteLine("Movie has been added.");
        }
        else
        {
            Console.WriteLine("Something went wrong.");
        }
    }

    public void DeleteMovie(Movie movie)
    {
        if (_movieRepo.DeleteMovie(movie))
        {
            Console.WriteLine("\nMovie Deleted Successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public void ShowAllMovies()
    {
        List<Movie> movies = _movieRepo.GetAllMovies();
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
