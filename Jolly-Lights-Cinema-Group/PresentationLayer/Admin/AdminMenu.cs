using Gtk;
using Jolly_Lights_Cinema_Group.BusinessLogic;
using Jolly_Lights_Cinema_Group.Models;
using Jolly_Lights_Cinema_Group.Presentation;

namespace Jolly_Lights_Cinema_Group
{
    public static class AdminMenu
    {
        private static Menu _adminMenu = new("Admin Menu", new string[] { "Manage Reservations", "Manage Users", "Manage Locations", "Manage Movies", "Manage shop", "Manage Movie Schedule", "Manage Rooms", "Manage Seat Prices", "View Reports", "Settings", "Logout" });
        public static void ShowAdminMenu(ref User user)
        {
            var inAdminMenu = true;
            Console.Clear();

            while (inAdminMenu)
            {
                int userChoice = _adminMenu.Run();
                inAdminMenu = HandleAdminChoice(userChoice, ref user);
                Console.Clear();
            }
        }

        public static bool HandleAdminChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    ReservationMenu.ShowReservationMenu();
                    return true;
                case 1:
                    ManageEmployeeMenu.ShowManageEmployeeMenu();
                    return true;
                case 2:
                    ManageLocationMenu.ShowManageLocationMenu();
                    return true;
                case 3:
                    AdminMovieHandler.ManageMovies();
                    return true;
                case 4:
                    ShopManagementHandler.ManageShopManagement();
                    return true;
                case 5:
                    MovieScheduleHandler.ScheduleMovies();
                    return true;
                case 6:
                    AdminMovieRoomHandler.ManageMovieRooms();
                    return true;
                case 7:
                    SeatService.ManageSeats();
                    return true;
                case 8:
                    ReportsHandler.ManageReports();
                    return true;
                case 9:
                    AccountSettingsMenu.ShowAccountSettingsMenu();
                    return true;
                case 10:
                    user.IsAuthenticated = false;
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return false;
            }
        }
    }
}
