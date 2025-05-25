using Jolly_Lights_Cinema_Group;

public static class ManageMovieMenu
{
    private static MovieService _movieService = new();
    private static Menu _manageMovieMenu = new("Movie Management Menu.", new string[] { "Add Movie", "View Movies", "Back" });
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
        string? inputReleasedate;
        do
        {
            Console.Write("Enter release date of the movie (dd-MM-yyyy): ");
            inputReleasedate = Console.ReadLine();
        } while (!DateTime.TryParse(inputReleasedate, out releaseDate));

        Console.WriteLine("who are in the movie cast?");
        string movieCast = Console.ReadLine()!;

        Movie movie = new(title, duration, minimumAge, releaseDate, movieCast);

        Console.Clear();

        Console.Clear();
        Console.WriteLine($"Title: {movie.Title}\nDuration: {movie.Duration}\nMinimum Age: {movie.MinimumAge}\nReleaseDate: {movie.ReleaseDate}\nMoviecast: {movie.MovieCast}");
        Console.WriteLine("\nEnter y to add movie to the database: ");
        string? input = Console.ReadLine();

        if (input != null && input.Trim().ToLower() == "y")
        {
            if (_movieService.RegisterMovie(movie))
            {
                Console.WriteLine($"Movie added succesfully");
            }
            else
            {
                Console.WriteLine($"Movie could not be added to the database");
            }
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
                Console.WriteLine($"Title: {movie.Title}, Duration: {movie.Duration} minutes, MinimumAge: {movie.MinimumAge}, ReleaseDate: {movie.ReleaseDate.ToString("dd-MM-yyyy")}, Cast: {movie.MovieCast}");
            }
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}
