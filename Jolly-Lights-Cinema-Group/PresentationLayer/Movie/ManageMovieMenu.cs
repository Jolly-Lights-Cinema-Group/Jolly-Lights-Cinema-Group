using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Helpers;

public static class ManageMovieMenu
{
    private static MovieService _movieService = new();
    private static Menu _manageMovieMenu = new("Movie Management Menu.", new string[] { "Add Movie", "View Movies", "Edit Movie", "Back" });
    public static void ShowManageMovieMenu()
    {
        bool inManageMovieMenu = true;
        Console.Clear();

        while (inManageMovieMenu)
        {
            int userChoice = _manageMovieMenu.Run();
            inManageMovieMenu = HandleManageMovieChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleManageMovieChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                AddMovie();
                return true;
            case 1:
                ViewMovies();
                return true;
            case 2:
                EditMovie();
                return true;
            case 3:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    private static void AddMovie()
    {
        Console.Clear();

        Console.WriteLine("Information to add movie");

        string? title;
        do
        {
            Console.Write("Enter movie title: ");
            title = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(title));

        int duration;
        string? inputDuration;
        do
        {
            Console.Write("Enter duration of the movie in minutes: ");
            inputDuration = Console.ReadLine();
        } while (!int.TryParse(inputDuration, out duration) || duration < 0);

        int minimumAge;
        string? inputMinAge;
        do
        {
            Console.Write("Enter minimum age of the movie: ");
            inputMinAge = Console.ReadLine();
        } while (!int.TryParse(inputMinAge, out minimumAge) || minimumAge < 0);

        DateTime releaseDate;
        do
        {
            Console.WriteLine("Enter release date: ");
            string? inputReleasedate = Console.ReadLine();
            if (DateTimeValidator.TryParseDate(inputReleasedate, out releaseDate))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
            }
        } while (true);

        Console.WriteLine("who are in the movie cast?");
        string movieCast = Console.ReadLine()!;

        Movie movie = new(title, duration, minimumAge, releaseDate, movieCast);

        Console.Clear();

        Console.Clear();
        Console.WriteLine($"Title: {movie.Title}\nDuration: {movie.Duration}\nMinimum Age: {movie.MinimumAge}\nReleaseDate: {movie.ReleaseDate}\nMoviecast: {movie.MovieCast}");

        if (_movieService.RegisterMovie(movie))
        {
            Console.WriteLine($"Movie added succesfully");
        }
        else
        {
            Console.WriteLine($"Movie could not be added to the database");
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    public static void ViewMovies()
    {
        Console.Clear();

        List<Movie> movies = _movieService.ShowAllMovies();

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies found.");
        }
        else
        {
            Console.WriteLine("Movies:");
            foreach (var movie in movies)
            {
                Console.WriteLine($"Title: {movie.Title}, Duration: {movie.Duration} minutes, MinimumAge: {movie.MinimumAge}, ReleaseDate: {movie.ReleaseDate.ToString("dd/MM/yyyy")}, Cast: {movie.MovieCast}");
            }
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    public static void EditMovie()
    {
        Console.Clear();

        MovieService movieService = new();
        Movie? movie = null;
        string? title;

        do
        {
            Console.Write("Enter movie title to edit movie (or type 'C' to cancel): ");
            title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty.");
                continue;
            }

            if (title.Trim().ToLower() == "c")
            {
                return;
            }

            movie = movieService.GetMovieTitle(title);
            
            if (movie == null)
            {
                Console.WriteLine($"No movie found with title: {title}.");
            }

        } while (movie is null);


        Console.Clear();

            Console.Clear();
            Console.WriteLine($"Editing: {movie!.Title}");

            Console.Write($"New Title (leave empty to keep current: {movie.Title}): ");
            string? newTitle = Console.ReadLine();
            newTitle = string.IsNullOrWhiteSpace(newTitle) ? null : newTitle;

            Console.Write($"New Duration (leave empty to keep current: {movie.Duration} minutes): ");
            string? newDuration = Console.ReadLine();
            newDuration = string.IsNullOrWhiteSpace(newDuration) ? null : newDuration;

            Console.Write($"New Minimum Age (leave empty to keep current: {movie.MinimumAge}): ");
            string? newMinimumAge = Console.ReadLine();
            newMinimumAge = string.IsNullOrWhiteSpace(newMinimumAge) ? null : newMinimumAge;

            Console.Write($"New Release Date dd/MM/yyyy (leave empty to keep current: {movie.ReleaseDate.ToString("dd/MM/yyyy")}): ");
            string? newReleaseDate = Console.ReadLine();
            newReleaseDate = string.IsNullOrWhiteSpace(newReleaseDate) ? null : newReleaseDate;

            Console.Write($"New Movie Cast (leave empty to keep current: {movie.MovieCast}):\n");
            string? newCast = Console.ReadLine();
            newCast = string.IsNullOrWhiteSpace(newCast) ? null : newCast;

            Movie? newMovie = _movieService.UpdateMovie(movie, newTitle, newDuration, newMinimumAge, newReleaseDate, newCast);

            if (newMovie != null)
            {
                Console.Clear();
                Console.WriteLine($"{newMovie.Title} is updated");
                Console.WriteLine($"Title: {newMovie.Title}\nDuration: {newMovie.Duration} minutes\nMinimum Age: {newMovie.MinimumAge}\nRelease Date: {newMovie.ReleaseDate.ToString("dd/MM/yyyy")}\nMovie Cast: {newMovie.MovieCast}");
            }
            else Console.WriteLine("Movie could not be updated.");

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
    }
}
