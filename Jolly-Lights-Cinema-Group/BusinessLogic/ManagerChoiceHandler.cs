using Jolly_Lights_Cinema_Group.Domain;

namespace Jolly_Lights_Cinema_Group
{
    public class managerChoiceHandler
    {

// Class for the Manager menu to work with the logic placed in the program class.

        public static void HandleChoice(int choice, ref User user)
        {
            switch (choice)
            {
                case 0:
                    ManageUsers();
                    break;
                case 1:
                    ViewReports();
                    break;
                case 2:
                    AccessSettings();
                    break;
                case 3: 
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