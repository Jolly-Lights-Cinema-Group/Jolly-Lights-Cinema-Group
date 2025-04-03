using System.Net;
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
                    ManageUsers();
                    break;
                case 1:
                    AdminLocationHandler.ManageLocations();
                    break;
                case 2:
                    ViewReports();
                    break;
                case 3:
                    AccessSettings();
                    break;
                case 4:
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
            UserRole role;
            Console.Clear();
            Console.WriteLine("What is the firstname of the user?");
            string firstName = Console.ReadLine()!;
            Console.WriteLine("What is the Lastname of the user?");
            string lastName = Console.ReadLine()!;
            Console.WriteLine("What is the email of the user?");
            string email = Console.ReadLine()!;
            Console.WriteLine("What will the username be of the user?");
            string username = Console.ReadLine()!;
            Console.WriteLine("What will the password be of the user?");
            string password = Console.ReadLine()!;
            Console.WriteLine("What will the role be of the user? \nEmployee\nAdmin\nManager");
            string StrRole = Console.ReadLine()!;

            switch (StrRole)
            {
                case "Employee":
                role = UserRole.Employee;
                break;
                case "Admin":
                role = UserRole.Admin;
                break;
                case "Manager":
                role = UserRole.Manager;
                break;
                default:
                Console.WriteLine("Invalid role input");
                return;
            }
            
            EmployeeService employeeService = new EmployeeService();

            if (employeeService.RegisterEmployee(firstName,lastName,email,username,password,role))
            {
            Console.WriteLine($"User created: {firstName} {lastName}, Email: {email}, Username: {username}, Role: {StrRole}");
            }
            else
            {
                Console.WriteLine("User Creating failed.");
            }

            Console.ReadKey();
        }


        private static void DeleteEmployee()
        {
            Console.Clear();
            Console.WriteLine("You can delete users by typing the first and lastname of the employee.");
            Console.WriteLine("Firstname:");
            string firstname = Console.ReadLine()!;
            Console.WriteLine("Lastname: ");
            string lastname = Console.ReadLine()!;

            EmployeeService employeeService = new EmployeeService();
            employeeService.DeleteEmployee(firstname,lastname);

            Console.ReadKey();
        }

        private static void ViewAllEmployees()
        {
            Console.Clear();
            EmployeeService employeeService = new EmployeeService();
            employeeService.ShowAllEmployees();
            Console.ReadKey();
        }
    }
}