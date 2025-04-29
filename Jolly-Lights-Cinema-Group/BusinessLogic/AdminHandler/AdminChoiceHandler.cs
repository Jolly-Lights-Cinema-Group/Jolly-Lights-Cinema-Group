using Jolly_Lights_Cinema_Group.Models;

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
                    ReservationHandler.ManageReservations();
                    break;
                case 1:
                    AdminUserHandler.ManageUsers();
                    break;
                case 2:
                    AdminLocationHandler.ManageLocations();
                    break;
                case 3:
                    AdminMovieHandler.ManageMovies();
                    break;
                case 4:
                    ViewReports();
                    break;
                case 5:
                    AccountSettingsHandler.ManageAccount();
                    break;
                case 6:
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
    }
}