using System.Net;
using System.Security.Cryptography.X509Certificates;
using Jolly_Lights_Cinema_Group.Domain;
using JollyLightsCinemaGroup.BusinessLogic;

namespace Jolly_Lights_Cinema_Group
{
    public class AdminChoiceHandler
    {

// Class for the Admin menu to work with the logic placed in the main program.

        public static bool AdminMainMenu = false;
        public static bool AdminManageUserMenu = false;
        public static void HandleChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    AdminUserHandler.ManageUsers();
                    break;
                case 1:
                    AdminLocationHandler.ManageLocations();
                    break;
                case 2:
                    AdminMovieHandler.ManageMovies();
                    break;
                case 3:
                    ViewReports();
                    break;
                case 4:
                    AccessSettings();
                    break;
                case 5:
                    user.IsAuthenticated = false;
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
        }

        private static void ViewReports()
        {
            Console.Clear();
            Console.WriteLine("Viewing reports...");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private static void AccessSettings()
        {
            Console.Clear();
            Console.WriteLine("Accessing settings...");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}