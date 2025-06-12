using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;

public static class ManageEmployeeMenu
{
    private static EmployeeService _employeeService = new();
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
        Console.Clear();

        string? firstName;
        do
        {
            Console.Write("Enter first name: ");
            firstName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName))
                Console.WriteLine("First name cannot be empty. Please try again.");

        } while (string.IsNullOrWhiteSpace(firstName));

        string? lastName;
        do
        {
            Console.Write("Enter last name: ");
            lastName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(lastName))
                Console.WriteLine("Last name cannot be empty. Please try again.");

        } while (string.IsNullOrWhiteSpace(lastName));

        string? dateOfBirthInput;
        DateTime dateOfBirth;
        do
        {
            Console.Write("Enter date of birth: ");
            dateOfBirthInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(dateOfBirthInput))
            {
                Console.WriteLine("Date of birth cannot be empty. Please try again.");
                continue;
            }

            if (!DateTime.TryParse(dateOfBirthInput, out dateOfBirth))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date (e.g. 22/02/2025).");
                continue;
            }

            if (dateOfBirth > DateTime.Now)
            {
                Console.WriteLine("Date of birth cannot be in the future. Please enter a valid date.");
                continue;
            }
            break;
        } while (true);

        string? address;
        do
        {
            Console.Write("Enter address: ");
            address = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Please Type in a valid adress like: Straatweg 99");
            }
        } while (string.IsNullOrWhiteSpace(address));

        string? eMail;
        do
        {
            Console.Write("Enter email address: ");
            eMail = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(eMail) || !eMail.Contains("@"))
            {
                Console.WriteLine("Make sure that the email contains a '@'. Also the email can't be empty.");
            }
        } while (string.IsNullOrWhiteSpace(eMail) || !eMail.Contains("@"));

        string? userName;
        do
        {
            Console.Write("Enter username: ");
            userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username can't be empty");
            }

            if (_employeeService.UserNameExists(userName))
            {
                Console.WriteLine("Username already exists. Try another one");
            }
        } while (string.IsNullOrWhiteSpace(userName) || _employeeService.UserNameExists(userName));

        string? password;
        do
        {
            Console.Write("Enter password: ");
            password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password can't be empty");
            }
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

        Employee employee = new Employee(firstName, lastName, dateOfBirthInput, address, eMail, userName, password, role);

        if (_employeeService.RegisterEmployee(employee))
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

    public static void DeleteEmployee()
    {
        Console.Clear();
        Console.WriteLine("Delete employee.");

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

        string? userName;
        do
        {
            Console.Write("Enter username: ");
            userName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(userName));

        Employee? employee = _employeeService.GetEmployeeByUserName(userName, firstName, lastName);
        Console.Clear();

        if (employee is null)
        {
            Console.WriteLine($"No employee found.");
        }
        else
        {
            Console.WriteLine($"Delete Employee:\nFirstname: {employee.FirstName}, Lastname: {employee.LastName}");
            Console.WriteLine($"\nEnter y to confirm: ");
            string? input = Console.ReadLine();
            if (input != null && input.Trim().ToLower() == "y")
            {
                if (_employeeService.DeleteEmployee(employee))
                {
                    Console.Clear();
                    Console.WriteLine($"Employee deleted succesfully");
                }
                else Console.WriteLine($"Employee could not be deleted");
            }

            else
            {
                Console.WriteLine($"Employee deletion cancelled");
            }
        }

        Console.WriteLine($"\nPress any key to continue");
        Console.ReadKey();
    }

    public static void ViewAllEmployees()
    {
        Console.Clear();
        List<Employee> employees = _employeeService.ShowAllEmployees();
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
        }
        else
        {
            Console.WriteLine("Employees:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"Firstname: {emp.FirstName}, Lastname: {emp.LastName}, Date of birth: {emp.DateofBirth}, Adress: {emp.Address}, Email: {emp.Email}, Username: {emp.UserName}, Role: {emp.Role}");
            }
        }
        Console.ReadKey();
    }
}