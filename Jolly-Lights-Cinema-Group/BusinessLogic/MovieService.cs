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

        _movieRepo.AddMovie(movie.Title, movie.Duration, movie.MinimumAge);
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
