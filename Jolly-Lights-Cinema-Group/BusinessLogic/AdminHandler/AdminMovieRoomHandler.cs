using Gtk;
using Jolly_Lights_Cinema_Group.Helpers;
using Jolly_Lights_Cinema_Group.Common;

namespace Jolly_Lights_Cinema_Group
{
    public class AdminMovieRoomHandler
    {
        public static void ManageMovieRooms()
        {
            bool inManageMovieRoomMenu = true;
            AdminManageMovieRoomMenu ManageMovieRoomMenu = new();
            Console.Clear();
            
            while(inManageMovieRoomMenu)
            {
                int userChoice = ManageMovieRoomMenu.Run();
                inManageMovieRoomMenu = HandleManageMovieRoomChoice(userChoice);
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
        
        [STAThread]
        public static void AddMovieRoom()
        {
            Console.Clear();
            Console.WriteLine("What is the number of the room?");
            var number = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What movieType will be viewed in de room");
            var movieType = Convert.ToInt32(Console.ReadLine());

            var locationId = 0;

            if (Globals.CurrentUser?.Location?.Id > 0)
                locationId = (int)Globals.CurrentUser.Location!.Id;
            else
            {
                Console.WriteLine("Enter locationId for the room");
                locationId = Convert.ToInt32(Console.ReadLine());
            }

            var inputFilePath = SelectFile();
            if (inputFilePath == null)
            {
                Console.WriteLine("Geen bestand geselecteerd. Programma wordt afgesloten.");
                return;
            }
            
            var grid = MovieRoomJsonHelper.ReadGridFromFile(inputFilePath);
            var json = MovieRoomJsonHelper.ConvertGridToJson(grid);
            
            var movieRoomService = new MovieRoomService();
            movieRoomService.RegisterMovieRoom(number, json, movieType, locationId);
        }

        public static void DeleteMovieRoom()
        {
            Console.Clear();
            
            Console.WriteLine("What is the number of the room?");
            var number = Convert.ToInt32(Console.ReadLine());
            
            var locationId = 0;

            if (Globals.CurrentUser?.Location?.Id > 0)
                locationId = (int)Globals.CurrentUser.Location.Id;
            else
            {
                Console.WriteLine("Enter locationId for the room");
                locationId = Convert.ToInt32(Console.ReadLine());
            }
            
            var movieRoomService = new MovieRoomService();
            movieRoomService.DeleteRoom(number, locationId);

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        public static void ViewAllMoviesRooms()
        {
            var locationId = 0;

            if (Globals.CurrentUser?.Location?.Id > 0)
                locationId = (int)Globals.CurrentUser.Location!.Id;
            else
            {
                Console.WriteLine("Enter locationId for the room");
                locationId = Convert.ToInt32(Console.ReadLine());
            }
            
            var movieRoomService = new MovieRoomService();
            movieRoomService.ShowMoviesRoomsLocation(locationId);
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
                selectedPath = fileChooser.Filename;
                Console.WriteLine($"Selected file: {selectedPath}");
            }

            fileChooser.Destroy();
            win.Destroy();
            Application.Quit();

            return selectedPath;
        }
    }
}