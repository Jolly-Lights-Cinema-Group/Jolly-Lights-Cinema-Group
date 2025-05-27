using Jolly_Lights_Cinema_Group;
using Gtk;
using Menu = Jolly_Lights_Cinema_Group.Menu;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;
using Jolly_Lights_Cinema_Group.Helpers;

public static class ManageMovieRoomMenu
{
    private static MovieRoomService _movieRoomService = new();
    private static Menu _manageMovieRoomnMenu = new("Movie room Management Menu.", new string[] { "Add Movie room", "Delete Movie room", "View Movie rooms", "Back" });
    public static void ShowManageMovieRoomMenu()
    {
        bool inManageMovieRoomnMenu = true;
        Console.Clear();

        while (inManageMovieRoomnMenu)
        {
            int userChoice = _manageMovieRoomnMenu.Run();
            inManageMovieRoomnMenu = HandleManageMovieRoomChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleManageMovieRoomChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                AddMovieRoom();
                return true;
            case 1:
                DeleteMovieRoom();
                return true;
            case 2:
                ViewAllMoviesRooms();
                return true;
            case 3:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    private static void AddMovieRoom()
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

        List<MovieRoom> existingRooms = _movieRoomService.GetMovieRooms(locationId);

        int roomNumber;
        string? inputRoomNumber;
        do
        {
            Console.Write("Enter roomnumber: ");
            inputRoomNumber = Console.ReadLine();

            if (!int.TryParse(inputRoomNumber, out roomNumber) || roomNumber < 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
                continue;
            }

            if (existingRooms.Any(r => r.RoomNumber == roomNumber))
            {
                Console.WriteLine("Room number already exists at this lcoation. Enter a different number.");
                inputRoomNumber = null;
            }

        } while (inputRoomNumber == null || !int.TryParse(inputRoomNumber, out roomNumber) || roomNumber < 0);

        string[] movieTypeOtions = { "Regular", "3D", "Dolby", "Dolby Imax" };
        Menu movieTypeMenu = new("What movie type can be viewed in the room:", movieTypeOtions);

        int selectedIndex = movieTypeMenu.Run();

        MovieType movieType;
        switch (selectedIndex)
        {
            case 0:
                movieType = MovieType.Regular;
                break;
            case 1:
                movieType = MovieType._3D;
                break;
            case 2:
                movieType = MovieType.Dolby;
                break;
            case 3:
                movieType = MovieType.DolbyImax;
                break;
            default:
                Console.WriteLine("Invalid selection.");
                return;
        }

        var inputFilePath = SelectFile();
        if (inputFilePath == null)
        {
            Console.WriteLine("No file selected programm will be closed");
            return;
        }

        var grid = MovieRoomJsonHelper.ReadGridFromFile(inputFilePath);
        var json = MovieRoomJsonHelper.ConvertGridToJson(grid);

        if (string.IsNullOrWhiteSpace(json)) Console.WriteLine("Error: Room lay out cannot be empty.");

        else
        {
            MovieRoom movieRoom = new(roomNumber, json, movieType, locationId);

            Console.Clear();

            if (_movieRoomService.RegisterMovieRoom(movieRoom))
            {
                Console.Write("Movieroom succesfully added");
            }
            else Console.WriteLine("Movieroom could not be added");
        }

        Console.WriteLine($"\nPress any key to continue");
        Console.ReadKey();
    }

    private static string? SelectFile()
    {
        var selectedPath = "";

        Application.Init();
        var win = new Window("File Picker");
        win.SetDefaultSize(500, 300);

        var fileChooser = new FileChooserDialog(
            "Choose a file to open",
            win,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept);


        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            try
            {
                var fileUri = fileChooser.Uri;

                var uri = new Uri(fileUri);
                selectedPath = uri.LocalPath;

                Console.WriteLine($"Selected file: {selectedPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while trying to fetch filepath: {ex.Message}");
            }
        }

        fileChooser.Destroy();
        win.Destroy();
        Application.Quit();

        return selectedPath;
    }

    public static void DeleteMovieRoom()
    {
        Console.Clear();
        Console.WriteLine("Delete Movieroom:");

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

        List<MovieRoom> existingRooms = _movieRoomService.GetMovieRooms(locationId);

        string[] movieRoomMenuItems = existingRooms
            .Select(movieRoom => $"Room number: {movieRoom.RoomNumber}, Supported movie type: {movieRoom.SupportedMovieType.ToString()}")
            .Append("Cancel")
            .ToArray();

        Menu movieRoomMenu = new("Select a movie room:", movieRoomMenuItems);
        int movieRoomChoice = movieRoomMenu.Run();

        if (movieRoomChoice >= existingRooms.Count)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        MovieRoom selectedMovieRoom = existingRooms[movieRoomChoice];

        Console.Clear();

        if (_movieRoomService.DeleteRoom(selectedMovieRoom))
        {
            Console.WriteLine("Deleted movie room succesfully");
        }
        else Console.WriteLine("Movie room could not be deleted");

        Console.WriteLine($"\nPress any key to continue");
        Console.ReadKey();
    }

    public static void ViewAllMoviesRooms()
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

        List<MovieRoom> movieRooms = _movieRoomService.GetMovieRooms(locationId);
        Console.Clear();

        if (movieRooms.Count == 0)
        {
            Console.WriteLine("No movie rooms found.");
        }
        else
        {
            Console.WriteLine("Movie rooms:");
            foreach (MovieRoom movieRoom in movieRooms)
            {
                Console.WriteLine($"Room number: {movieRoom.RoomNumber}, Supported movie type: {movieRoom.SupportedMovieType.ToString()}");
            }
        }

        Console.WriteLine($"\nPress any key to continue");
        Console.ReadKey();
    }
}
