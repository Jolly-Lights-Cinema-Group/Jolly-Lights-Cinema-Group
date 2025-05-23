using Jolly_Lights_Cinema_Group.Models;
using Jolly_Lights_Cinema_Group.Presentation;

namespace Jolly_Lights_Cinema_Group
{
    public class ManagerChoiceHandler
    {

        // Class for the Manager menu to work with the logic placed in the program class.

        public static void HandleChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    ReservationMenu.ShowReservationMenu();
                    break;
                case 1:
                    DiscountCodeHandler.ManageDiscountCode();
                    break;
                case 2:
                    ShopManagementHandler.ManageShopManagement();
                    break;
                case 3:
                    ReportsHandler.ManageReports();
                    break;
                case 4:
                    AccountSettingsMenu.ShowAccountSettingsMenu();
                    break;
                case 5:
                    user.IsAuthenticated = false;
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
        }
    }
}