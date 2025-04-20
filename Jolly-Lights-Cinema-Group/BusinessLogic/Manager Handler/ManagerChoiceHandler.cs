using Jolly_Lights_Cinema_Group.Models;

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
                    ManageReservations();
                    break;
                case 1:
                    Console.WriteLine("Need to be implemented.");
                    break;
                case 2:
                    Console.WriteLine("Need to be implemented.");
                    break;
                case 3:
                    AccountSettingsHandler.ManageAccount();
                    break;
                case 4:
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

        private static void ManageReservations()
        {
            Console.Clear();
            Console.WriteLine("Manage Reservations");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}