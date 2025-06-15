using Jolly_Lights_Cinema_Group;

public class MovieScheduleMenu
{
    private readonly MovieService _movieService;

    public MovieScheduleMenu()
    {
        _movieService = new MovieService();
    }

    public Movie? SelectMovieMenu(int locationId)
    {
        ScheduleService scheduleService = new();
        List<DateTime>? birthDates = null;

        while (true)
        {
            List<Movie> scheduledMovies = scheduleService.GetMoviesBySchedule(locationId);

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

            Movie selectedMovie = scheduledMovies[movieChoice];

            if (selectedMovie.MinimumAge >= 16)
            {
                if (birthDates == null) birthDates = AgeVerifier.AskDateOfBirth(selectedMovie.MinimumAge.Value);

                if (!AgeVerifier.IsOldEnough(birthDates, selectedMovie.MinimumAge.Value))
                {
                    Console.WriteLine($"You must be at least {selectedMovie.MinimumAge} years old to watch {selectedMovie.Title}.");
                    Console.WriteLine("Press any key to choose a difrent movie.");
                    Console.ReadKey();
                    continue;
                }
            }

            return selectedMovie;
        }
    }
}
