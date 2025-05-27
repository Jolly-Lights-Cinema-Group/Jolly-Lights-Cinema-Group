using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;

public static class ManageScheduleMenu
{
    private static ScheduleService _scheduleService = new();
    private static Menu _manageScheduleMenu = new("Schedule Menu", new string[] { "Daily Manual planning", "Deleting planning", "Show Schedule by Date", "Automatic planning (Coming soon)", "Back" });
    public static void ShowScheduleManagementMenu()
    {
        bool inScheduleManagementMenu = true;
        Console.Clear();

        while (inScheduleManagementMenu)
        {
            int userChoice = _manageScheduleMenu.Run();
            inScheduleManagementMenu = HandleScheduleManagementChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleScheduleManagementChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                ManualPlanning();
                return true;
            case 1:
                DeleteSchedule();
                return true;
            case 2:
                ShowScheduleByDate();
                return true;
            case 3:
                return true;
            default:
                return false;
        }
    }

    private static void ManualPlanning()
    {
        Console.Clear();

        MovieService movieService = new();
        List<Movie> allMovies = movieService.ShowAllMovies();

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

        MovieRoomService movieRoomService = new();
        List<MovieRoom> movieRooms = movieRoomService.GetMovieRooms(Globals.SessionLocationId);

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

        Schedule schedule = new(selectedMovieRoom.RoomNumber, selectedMovie.Id!.Value, startDate, startTime);
        if (_scheduleService.CanAddSchedule(selectedMovieRoom.Id!.Value, startDate, startTime))
        {
            if (_scheduleService.RegisterSchedule(schedule) && _scheduleService.UpdateFreeTimeColumn())
            {
                Console.Clear();
                Console.WriteLine("Movie schedule successfully added!");
            }
            else Console.WriteLine("Schedule could not be added");
        }
        else Console.WriteLine("Schedule overlaps with another movie in the same room.");

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

    public static void DeleteSchedule()
    {
        Console.Clear();
        Console.WriteLine("Delete schedule");

        string? title;
        do
        {
            Console.Write("Enter movie title: ");
            title = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(title));

        MovieService movieService = new();
        Movie? movie = movieService.GetMovieTitle(title);

        if (movie is null) Console.WriteLine($"No movie found with title {title}");

        else
        {
            List<Schedule> movieSchedules = _scheduleService.GetSchedulesByMovie(movie);
            if (movieSchedules.Count <= 0)
            {
                Console.WriteLine($"No movie schedules found for movie: {title}");
            }

            else
            {
                MovieRoomService movieRoomService = new();
                List<MovieRoom> movieRooms = movieRoomService.GetMovieRooms(Globals.SessionLocationId);

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

                List<Schedule> schedules = _scheduleService.GetSchedulesByMovieAndRoom(movie, selectedMovieRoom);

                string[] scheduleItems = schedules
                    .Select(schedule => $"Date: {schedule.StartDate.ToString("dd-MM-yyyy")}, Time: {schedule.StartTime.ToString(@"hh\:mm")}")
                    .Append("Cancel")
                    .ToArray();

                Menu schedulesMenu = new($"Select a schedule for {movie.Title}, in roomnumber {selectedMovieRoom.RoomNumber}:", scheduleItems);
                int scheduleChoice = schedulesMenu.Run();

                if (scheduleChoice >= schedules.Count)
                {
                    Console.WriteLine("Cancelled.");
                    return;
                }

                Schedule selectedSchedule = schedules[scheduleChoice];
                if (_scheduleService.DeleteSchedule(selectedSchedule))
                {
                    Console.Clear();
                    Console.WriteLine("Schedule deleted succesfully.");
                }
                else Console.WriteLine("Schedule could not be deleted.");
            }
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

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

        List<Schedule> schedules = _scheduleService.ShowScheduleByDate(SearchDate);

        if (schedules.Count == 0)
        {
            Console.WriteLine("No schedules where found.");
            return;
        }

        else
        {
            Console.WriteLine($"Schedule Movies on {SearchDate.ToString("dd-MM-yyyy")}:");

            MovieService movieService = new();

            foreach (Schedule schedule in schedules)
            {
                Movie? movieinformation = movieService.GetMovieById(schedule.MovieId);
                Console.WriteLine($"Room: {schedule.MovieRoomId}, Movie: {movieinformation!.Title}, Date: {schedule.StartDate.ToString("dd-MM-yyyy")}, Time: {schedule.StartTime}");
            }
        }

        Console.WriteLine($"\nPress any key to continue");
        Console.Read();
    }
}
