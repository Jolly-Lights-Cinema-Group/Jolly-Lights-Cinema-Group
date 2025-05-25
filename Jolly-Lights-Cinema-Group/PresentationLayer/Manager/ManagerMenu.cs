using Jolly_Lights_Cinema_Group.Models;
using Jolly_Lights_Cinema_Group.Presentation;

namespace Jolly_Lights_Cinema_Group
{
    public static class ManagerMenu
    {
        private static Menu _managerMenu = new("Manager Menu", new string[] { "Manage Reservations", "Manage discount", "Manage Shop", "View Reports", "Settings", "Logout" });
        public static void ShowManagerMenu(ref User user)
        {
            var inManagerMenu = true;
            Console.Clear();

            while (inManagerMenu)
            {
                int userChoice = _managerMenu.Run();
                inManagerMenu = HandleManagerChoice(userChoice, ref user);
                Console.Clear();
            }
        }

        public static bool HandleManagerChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    ReservationMenu.ShowReservationMenu();
                    return true;
                case 1:
                    ManageDiscountCodeMenu.ShowManageDiscountCodeMenu();
                    return true;
                case 2:
                    ShopManagementMenu.ShowShopManagementMenu();
                    return true;
                case 3:
                    ReportsMenu.ShowReportsMenu();
                    return true;
                case 4:
                    AccountSettingsMenu.ShowAccountSettingsMenu();
                    return true;
                case 5:
                    user.IsAuthenticated = false;
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return false;
            }
        }
    }
}
