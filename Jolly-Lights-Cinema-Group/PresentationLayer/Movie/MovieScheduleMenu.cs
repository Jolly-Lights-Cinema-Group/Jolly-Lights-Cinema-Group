using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;

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
        DateTime? birthDate = null;

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

            if (selectedMovie.MinimumAge >= 18)
            {
                if (birthDate == null)
                    birthDate = AgeVerifier.AskDateOfBirth();

                if (!AgeVerifier.IsOldEnough(birthDate.Value, selectedMovie.MinimumAge.Value))
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
