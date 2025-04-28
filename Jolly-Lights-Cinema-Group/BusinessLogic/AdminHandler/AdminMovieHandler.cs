using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Jolly_Lights_Cinema_Group
{
    public static class AdminMovieHandler
    {
        public static void ManageMovies()
        {
            bool inManageMovieMenu = true;
            AdminManageMovieMenu ManageMovieMenu = new();
            Console.Clear();

            while (inManageMovieMenu)
            {
                int userChoice = ManageMovieMenu.Run();
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
                    DeleteMovie();
                    return true;
                case 2:
                    ViewAllMovies();
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
            bool AddingMovie = true;
            do
            {
                Console.Clear();
                Console.WriteLine("What is the title of the movie?");
                string Title = Console.ReadLine()!;
                Console.WriteLine("What is the duration in minutes of the movie?");
                int Duration = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("What is the minimum Age of the movie?");
                int MinimumAge = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("What is the releasedate of the movie?");
                DateTime ReleaseDate = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("who are in the movie cast?");
                string MovieCast = Console.ReadLine()!;

                Console.WriteLine("Are you ready to add the next movie to the database?\ny/n or e for exit: ");
                Console.WriteLine($"Title: {Title}\nDuration: {Duration}\nMinimum Age: {MinimumAge}\nReleaseDate: {ReleaseDate}\nMoviecast: {MovieCast}");
                Console.Write("Answer: ");
                string input = Console.ReadLine()!.ToLower();

                switch (input)
                {
                    case "y":
                        Movie movie = new Movie(Title, Duration, MinimumAge, ReleaseDate, MovieCast);
                        MovieService movieService = new MovieService();
                        movieService.RegisterMovie(movie);
                        Console.WriteLine("\nPress any key to exit.");
                        Console.ReadKey();
                        AddingMovie = false;
                        break;
                    case "n":
                        break;
                    case "e":
                        AddingMovie = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            } while (AddingMovie == true);
        }

        private static void DeleteMovie()
        {
            Console.Clear();
            Console.WriteLine("What is the title of the movie te remove?");
            string title = Console.ReadLine()!;
            Movie movietodelete = new Movie(title, 0, 0, DateTime.Now, "a");

            MovieService movieService = new MovieService();
            movieService.DeleteMovie(movietodelete);

            Console.ReadKey();
        }

        private static void ViewAllMovies()
        {
            Console.Clear();
            MovieService movieservice = new MovieService();
            movieservice.ShowAllMovies();
            Console.ReadKey();
        }
    }

}