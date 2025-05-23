using Jolly_Lights_Cinema_Group;
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
}
