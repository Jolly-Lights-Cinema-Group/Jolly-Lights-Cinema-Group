using Jolly_Lights_Cinema_Group.Models;
using Jolly_Lights_Cinema_Group.Presentation;

namespace Jolly_Lights_Cinema_Group
{
    public class EmployeeChoiceHandler
    {

// Class for the Employee menu to work with the logic placed in the program class.

        public static void HandleChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    ReservationMenu.ShowReservationMenu();
                    break;
                case 1:
                    ShopManagementHandler.ManageShopManagement();
                    break;
                case 2:
                    ManageUsers();
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

        private static void ManageUsers()
        {
            Console.Clear();
            Console.WriteLine("Managing users...");
            Console.WriteLine("Rick");
            Console.WriteLine("Pieter");
            Console.WriteLine("Sofie");

            
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey(); 
        }

        private static void ViewReports()
        {
            Console.WriteLine("Viewing reports...");
        }

        private static void AccessSettings()
        {
            Console.WriteLine("Accessing settings...");
        }
    }
}