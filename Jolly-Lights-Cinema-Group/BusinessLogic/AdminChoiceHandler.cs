using System.Net;
using Jolly_Lights_Cinema_Group.Domain;

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

        // Admin Main Menu Methods
        private static void ManageUsers()   // Makes new Menu with UserManagement rules.
        {
            bool inManageUsersMenu = true;
            AdminManageUsersMenu manageUsersMenu = new();
            Console.Clear();
            
            while(inManageUsersMenu)
            {
            int userChoice = manageUsersMenu.Run();
            inManageUsersMenu = HandleManageUserChoice(userChoice);
            Console.Clear();
            }
        }

        private static void ViewReports()
        {
            Console.WriteLine("Viewing reports...");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private static void AccessSettings()
        {
            Console.WriteLine("Accessing settings...");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        // Admin ManageUsers HandleManager
        private static bool HandleManageUserChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                AddEmployee();
                return true;
            case 1:
                DeleteEmployee();
                return true;
            case 2:
                ViewAllEmployees();
                return true;
            case 3:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }
        // Admin ManageUsers Methods
        private static void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("Information to add the user:");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }


        private static void DeleteEmployee()
        {
            Console.Clear();
            Console.WriteLine("Deletes user: USERINFORMATION");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private static void ViewAllEmployees()
        {
            Console.Clear();
            Console.WriteLine("All Employees:\n");
            Console.WriteLine("AAA");
            Console.WriteLine("BBB");
            Console.WriteLine("CCC");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}