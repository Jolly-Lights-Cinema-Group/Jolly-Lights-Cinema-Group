using Jolly_Lights_Cinema_Group.Models;
using Jolly_Lights_Cinema_Group.Presentation;

namespace Jolly_Lights_Cinema_Group
{
    public static class EmployeeMenu
    {
        private static Menu _employeeMenu = new("Employee Menu", new string[] { "Cash Desk", "Manage Reservations", "Manage Shop", "Account settings", "Logout" });
        public static void ShowEmployeerMenu(ref User user)
        {
            var inEmployeeMenu = true;
            Console.Clear();

            while (inEmployeeMenu)
            {
                int userChoice = _employeeMenu.Run();
                inEmployeeMenu = HandleEmployeeChoice(userChoice, ref user);
                Console.Clear();
            }
        }

        public static bool HandleEmployeeChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    CashDesk.ShowCashDeskMenu();
                    return true;
                case 1:
                    ReservationMenu.ShowReservationMenu();
                    return true;
                case 2:
                    ShopManagementMenu.ShowShopManagementMenu();
                    return true;
                case 3:
                    AccountSettingsMenu.ShowAccountSettingsMenu();
                    return true;
                case 4:
                    user.IsAuthenticated = false;
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return false;
            }
        }
    }
}
