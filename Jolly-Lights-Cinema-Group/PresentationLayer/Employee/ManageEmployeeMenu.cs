using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;

public static class ManageEmployeeMenu
{
    private static Menu _manageEmployeeMenu = new("Employee Management Menu.", new string[] { "Add new Employee", "Delete Employee", "View All Employees", "Back" });
    public static void ShowManageEmployeeMenu()
    {
        bool inManageEmployeeMenu = true;
        Console.Clear();

        while (inManageEmployeeMenu)
        {
            int userChoice = _manageEmployeeMenu.Run();
            inManageEmployeeMenu = HandleManageUserChoice(userChoice);
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
                // DeleteEmployee();
                return true;
            case 2:
                // ViewAllEmployees();
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
        Console.Clear();

        EmployeeService employeeService = new();

        string? firstName;
        do
        {
            Console.Write("Enter first name: ");
            firstName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(firstName));

        string? lastName;
        do
        {
            Console.Write("Enter last name: ");
            lastName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(lastName));

        string? dateOfBirth;
        do
        {
            Console.Write("Enter date of birth: ");
            dateOfBirth = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(dateOfBirth));

        string? address;
        do
        {
            Console.Write("Enter address: ");
            address = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(address));

        string? eMail;
        do
        {
            Console.Write("Enter email address: ");
            eMail = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(eMail) || !eMail.Contains("@"));

        string? userName;
        do
        {
            Console.Write("Enter username: ");
            userName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(userName) || employeeService.UserNameExists(userName));

        string? password;
        do
        {
            Console.Write("Enter password: ");
            password = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(password));

        Console.Clear();
        string[] roleOptions = { "Employee", "Admin", "Manager" };
        Menu roleMenu = new("Select the role for the employee:", roleOptions);

        int selectedIndex = roleMenu.Run();

        Role role;
        switch (selectedIndex)
        {
            case 0:
                role = Role.Employee;
                break;
            case 1:
                role = Role.Admin;
                break;
            case 2:
                role = Role.Manager;
                break;
            default:
                Console.WriteLine("Invalid selection.");
                return;
        }

        Employee employee = new Employee(firstName, lastName, dateOfBirth, address, eMail, userName, password, role);

        if (employeeService.RegisterEmployee(employee))
        {
            Console.Clear();
            Console.WriteLine($"Employee created: {employee.FirstName} {employee.LastName}\nEmail: {employee.Email}\nUsername: {employee.UserName}\nRole: {employee.Role.ToString()}");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("\nUser Creating failed.");
        }

        Console.ReadKey();
    }
}