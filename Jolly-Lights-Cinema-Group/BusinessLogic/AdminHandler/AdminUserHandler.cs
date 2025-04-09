using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;

namespace Jolly_Lights_Cinema_Group
{

    public static class AdminUserHandler
    {
        public static void ManageUsers()
        {
            bool inManageUsersMenu = true;
            AdminManageUsersMenu manageUsersMenu = new();
            Console.Clear();

            while (inManageUsersMenu)
            {
                int userChoice = manageUsersMenu.Run();
                inManageUsersMenu = HandleManageUserChoice(userChoice);
                Console.Clear();
            }
        }

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
        private static void AddEmployee()
        {
            Role role;
            Console.Clear();
            Console.WriteLine("What is the firstname of the Employee?");
            string firstName = Console.ReadLine()!;
            Console.WriteLine("What is the Lastname of the Employee?");
            string lastName = Console.ReadLine()!;
            Console.WriteLine("What is the Data of Birth of the Employee?");
            string DateofBirth = Console.ReadLine()!;
            Console.WriteLine("What is the Adress of the Employee?");
            string Address = Console.ReadLine()!;
            Console.WriteLine("What is the email of the Employee?");
            string email = Console.ReadLine()!;
            Console.WriteLine("What will the username be of the Employee?");
            string username = Console.ReadLine()!;
            Console.WriteLine("What will the password be of the Employee?");
            string password = Console.ReadLine()!;
            Console.WriteLine("What will the role be of the Employee?\n1: Employee\n2: Admin\n3: Manager");
            string StrRole = Console.ReadLine()!;

            switch (StrRole)
            {
                case "Employee":
                    role = Role.Employee;
                    break;
                case "Admin":
                    role = Role.Admin;
                    break;
                case "Manager":
                    role = Role.Manager;
                    break;
                default:
                    Console.WriteLine("Invalid role input");
                    return;
            }
            Employee employee = new Employee(firstName, lastName, DateofBirth, Address, email, username, password, role);
            EmployeeService employeeService = new EmployeeService();

            if (employeeService.RegisterEmployee(employee))
            {
                Console.WriteLine($"Employee created: {firstName} {lastName}\nEmail: {email},\nUsername: {username}\nRole: {StrRole}");
            }
            else
            {
                Console.WriteLine("\nUser Creating failed.");
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

            Employee employee = new Employee(firstname, lastname, "null", "null", "null", "null", "null", Role.Employee);
            EmployeeService employeeService = new EmployeeService();

            employeeService.DeleteEmployee(employee);

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
