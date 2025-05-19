using System.Data.Common;
using System.Reflection;
using Jolly_Lights_Cinema_Group.Common;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class MovieScheduleHandler
    {
        public static void ScheduleMovies()
        {
            bool inScheduleMovies = true;
            ScheduleMenu schedulemenu = new();
            Console.Clear();

            while (inScheduleMovies)
            {
                int userChoice = schedulemenu.Run();
                inScheduleMovies = HandleManageScheduleMenu(userChoice);
                Console.Clear();
            }
        }
        private static bool HandleManageScheduleMenu(int choice)
        {
            switch (choice)
            {
                case 0:
                    AddScheduleline();
                    return true;
                case 1:
                    DeleteScheduleLine();
                    return true;
                case 2:
                    ShowScheduleByDate();
                    return true;
                case 3:
                    return true;
                case 4:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }

        // adding movie schedule (making)  -- Movieroom (based on ID),moviename (based on id),startdate
        private static void AddScheduleline()
        {
            Console.Clear();
            Console.Clear();

            MovieRepository movieRepository = new();
            List<Movie> allMovies = movieRepository.GetAllMovies();

            string[] movieMenuItems = allMovies
                .Select(movie => $"Movie: {movie.Title}; Duration: {movie.Duration} minutes; Min Age: {movie.MinimumAge}")
                .Append("Cancel")
                .ToArray();

            Menu movieMenu = new("Select a movie to create a schedule:", movieMenuItems);
            int movieChoice = movieMenu.Run();

            if (movieChoice >= allMovies.Count)
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Movie selectedMovie = allMovies[movieChoice];

            MovieRoomRepository movieRoomRepository = new();
            List<MovieRoom> movieRooms = movieRoomRepository.GetAllMovieRooms(Globals.SessionLocationId);

            string[] movieRoomItems = movieRooms
                .Select(movieRoom => $"Roomnumber: {movieRoom.RoomNumber}")
                .Append("Cancel")
                .ToArray();

            Menu movieRoomMenu = new("Select a movie room:", movieRoomItems);
            int movieRoomChoice = movieRoomMenu.Run();

            if (movieRoomChoice >= movieRooms.Count)
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            MovieRoom selectedMovieRoom = movieRooms[movieRoomChoice];

            // Adding StartDate
            DateTime startDate;
            do
            {
                Console.Clear();
                Console.WriteLine("What is the start date? (dd/MM/yyyy)");
                string? inputDate = Console.ReadLine();
                if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out startDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
                }
            } while (true);

            // Adding StartTime
            TimeSpan startTime;
            do
            {
                Console.Clear();
                Console.WriteLine("What is the start time? (HH:mm:ss)");
                string? inputTime = Console.ReadLine();
                if (TimeSpan.TryParseExact(inputTime, "hh\\:mm\\:ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    out startTime))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use HH:mm:ss (e.g., 14:30:00).");
                }
            } while (true);

            ScheduleService scheduleService = new();

            Schedule schedule = new(selectedMovieRoom.RoomNumber, selectedMovie.Id!.Value, startDate, startTime);
            scheduleService.RegisterSchedule(schedule);
            Console.ReadKey();
        }

        // Deleting schedule line (delete)

        public static void DeleteScheduleLine()
        {
            Console.Clear();
            // Testdata for room: (next update has to come from DB)
            MovieRoom TestDataForMovieRoom = new(id: 1, roomNumber: 1, roomLayoutJson: "aa", supportedMovieType: 1, locationId: 1);

            ScheduleService scheduleservice = new();

            // Adding MovieId
            Movie? selectedMovie = null;
            do
            {
                Console.Clear();
                Console.WriteLine("From the movie DB, What movie do you want to Delete? (Title)");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int movieId))
                {
                    selectedMovie = MovieRepository.GetMovieById(movieId);
                    if (selectedMovie != null)
                    {
                        Console.WriteLine($"Movie selected: {selectedMovie.Title}");
                    }
                    else
                    {
                    }
                }
                else if (!string.IsNullOrWhiteSpace(input))
                {
                    selectedMovie = MovieRepository.GetMovieByTitle(input);
                    if (selectedMovie != null)
                    {
                        Console.WriteLine($"Movie selected: {selectedMovie.Title}");
                    }
                    else
                    {
                        Console.WriteLine("Movie not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid movie title or ID.");
                }
            } while (selectedMovie == null);


            // Adding RoomNumber
            int roomNumber = -1;
            do
            {
                Console.Clear();
                Console.WriteLine(" What is the room number?)");
                string? roomInput = Console.ReadLine();
                if (int.TryParse(roomInput, out roomNumber))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid room number.");
                }
            } while (roomNumber < 0);

            // Adding StartDate
            DateTime startDate;
            do
            {
                Console.Clear();
                Console.WriteLine("What is the start date? (dd/MM/yyyy)");
                string? inputDate = Console.ReadLine();
                if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out startDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
                }
            } while (true);

            // Adding StartTime
            TimeSpan startTime;
            do
            {
                Console.Clear();
                Console.WriteLine("What is the start time? (HH:mm:ss)");
                string? inputTime = Console.ReadLine();
                if (TimeSpan.TryParseExact(inputTime, "hh\\:mm\\:ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    out startTime))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use HH:mm:ss (e.g., 14:30:00).");
                }
            } while (true);

            Schedule schedule = new(roomNumber, selectedMovie.Id.Value, startDate.Date, startTime);
            scheduleservice.DeleteSchedule(schedule);
            Console.ReadKey();
        }

        // Show schedule
        public static void ShowScheduleByDate()
        {
            DateTime SearchDate;
            do
            {
                Console.Clear();
                Console.WriteLine("Search Date: (dd/MM/yyyy)");
                string? inputSearchDate = Console.ReadLine();
                if (DateTime.TryParseExact(inputSearchDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out SearchDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
                }
            } while (true);

            Console.Clear();
            ScheduleService scheduleservice = new();
            scheduleservice.ShowSchedule(SearchDate);
            Console.Read();
        }

        // Automatic schedule


    }
}