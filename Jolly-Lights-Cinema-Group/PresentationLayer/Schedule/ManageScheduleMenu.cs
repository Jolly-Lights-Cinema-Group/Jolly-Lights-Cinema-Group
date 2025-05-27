using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;
using Jolly_Lights_Cinema_Group.Helpers;

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
        int locationId;
        if (Globals.CurrentUser!.Role == Role.Admin)
        {

            LocationMenu location = new();
            int selectedLocation = location.Run();

            LocationService locationService = new LocationService();
            List<Location> locations = locationService.GetAllLocations();

            locationId = (int)locations[selectedLocation].Id!;
        }
        else locationId = Globals.SessionLocationId;

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
        List<MovieRoom> movieRooms = movieRoomService.GetMovieRooms(locationId);

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
            Console.WriteLine("What is the start date? (dd/MM/yyyy)");
            string? inputDate = Console.ReadLine();
            if (DateTimeValidator.TryParseDate(inputDate, out startDate))
            {
                if (startDate >= selectedMovie.ReleaseDate) break;

                else
                {
                    Console.WriteLine($"The start date cannot take place before the release date: {selectedMovie.ReleaseDate:dd/MM/yyyy}");
                }
            }

            else Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
        } while (true);

        TimeSpan startTime;
        while (true)
        {
            Console.WriteLine("What is the start time? (HH:mm:ss)");
            string? inputTime = Console.ReadLine();

            if (!DateTimeValidator.TryParseTime(inputTime, out startTime))
            {
                Console.WriteLine("Invalid format. Please use HH:mm:ss (e.g., 14:30:00).");
            }

            else if (!_scheduleService.CanAddSchedule(selectedMovieRoom.Id!.Value, startDate, startTime, selectedMovie.Id!.Value))
            {
                Console.WriteLine("Schedule overlaps with another movie in the same room.");
            }

            else break;
        }

        Schedule schedule = new(selectedMovieRoom.Id.Value, selectedMovie.Id!.Value, startDate, startTime);
        if (_scheduleService.RegisterSchedule(schedule) && _scheduleService.UpdateFreeTimeColumn())
        {
            Console.Clear();
            Console.WriteLine("Movie schedule successfully added!");
        }
        else Console.WriteLine("Schedule could not be added");

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

    public static void DeleteSchedule()
    {
        Console.Clear();
        Console.WriteLine("Delete schedule");

        int locationId;
        if (Globals.CurrentUser!.Role == Role.Admin)
        {

            LocationMenu location = new();
            int selectedLocation = location.Run();

            LocationService locationService = new LocationService();
            List<Location> locations = locationService.GetAllLocations();

            locationId = (int)locations[selectedLocation].Id!;
        }
        else locationId = Globals.SessionLocationId;

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
                List<MovieRoom> movieRooms = movieRoomService.GetMovieRooms(locationId);

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
        Console.Clear();

        int locationId;
        if (Globals.CurrentUser!.Role == Role.Admin)
        {

            LocationMenu location = new();
            int selectedLocation = location.Run();

            LocationService locationService = new LocationService();
            List<Location> locations = locationService.GetAllLocations();

            locationId = (int)locations[selectedLocation].Id!;
        }
        else locationId = Globals.SessionLocationId;

        DateTime SearchDate;
        do
        {
            Console.WriteLine("Search Date: (dd/MM/yyyy)");
            string? inputSearchDate = Console.ReadLine();
            if (DateTimeValidator.TryParseDate(inputSearchDate, out SearchDate))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
            }
        } while (true);

        Console.Clear();

        List<Schedule> schedules = _scheduleService.ShowScheduleByDate(SearchDate, locationId);
        schedules = schedules.OrderBy(s => s.StartTime).ToList();

        if (schedules.Count == 0)
        {
            Console.WriteLine("No schedules where found.");
        }

        else
        {
            Console.WriteLine($"Schedule Movies on {SearchDate.ToString("dd-MM-yyyy")}:");

            MovieService movieService = new();
            MovieRoomService movieRoomService = new();

            foreach (Schedule schedule in schedules)
            {
                Movie? movieinformation = movieService.GetMovieById(schedule.MovieId);
                MovieRoom? movieRoom = movieRoomService.GetMovieRoomById(schedule.MovieRoomId);

                Console.WriteLine($"Room number: {movieRoom!.RoomNumber}, Movie: {movieinformation!.Title}, Date: {schedule.StartDate.ToString("dd-MM-yyyy")}, Time: {schedule.StartTime}");
            }
        }

        Console.WriteLine($"\nPress any key to continue");
        Console.Read();
    }
}
