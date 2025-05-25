using JollyLightsCinemaGroup.DataAccess;

public class MovieService
{
    private readonly MovieRepository _movieRepo;
    public MovieService()
    {
        _movieRepo = new MovieRepository();
    }
    public bool RegisterMovie(Movie movie)
    {
        return _movieRepo.AddMovie(movie);
    }

    public List<Movie> ShowAllMovies()
    {
        return _movieRepo.GetAllMovies();
    }

    public Movie? GetMovieById(int id)
    {
        return _movieRepo.GetMovieById(id);
    }

    public Movie? GetMovieTitle(string title)
    {
        return _movieRepo.GetMovieByTitle(title);
    }
}
