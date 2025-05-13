using Jolly_Lights_Cinema_Group.Helpers;
using Jolly_Lights_Cinema_Group.Common;
using Gtk;
using System;
using System.Threading;

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

            Application.Init();
            
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
            string? selectedPath = null;

            Application.Invoke(delegate {
                using var openFileDialog = new FileChooserDialog(
                    "Selecteer een bestand",
                    null,
                    FileChooserAction.Open,
                    "Cancel", ResponseType.Cancel,
                    "Open", ResponseType.Accept
                );

                if (openFileDialog.Run() == (int)ResponseType.Accept)
                {
                    selectedPath = openFileDialog.Filename;
                }

                openFileDialog.Destroy();
            });

            return selectedPath;
        }
    }
}