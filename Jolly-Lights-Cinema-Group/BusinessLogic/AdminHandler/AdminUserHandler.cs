using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;

namespace Jolly_Lights_Cinema_Group
{

    public static class AdminUserHandler
    {
        private static bool HandleManageUserChoice(int choice)
        {
            switch (choice)
            {
                case 0:
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
