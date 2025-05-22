using Jolly_Lights_Cinema_Group;

public class MovieScheduleMenu
{
    private readonly MovieService _movieService;

    public MovieScheduleMenu()
    {
        _movieService = new MovieService();
    }

    public Movie? SelectMovieMenu()
    {
        ScheduleService scheduleService = new();
        List<Movie> scheduledMovies = scheduleService.GetMoviesBySchedule();

        string[] movieMenuItems = scheduledMovies
            .Select(movie => $"Movie: {movie.Title}; Duration: {movie.Duration} minutes; Min Age: {movie.MinimumAge}")
            .Append("Cancel")
            .ToArray();

        Menu movieMenu = new("Select a movie:", movieMenuItems);
        int movieChoice = movieMenu.Run();

        if (movieChoice >= scheduledMovies.Count)
        {
            Console.WriteLine("Cancelled.");
            return null;
        }

        return scheduledMovies[movieChoice];
    }
}
