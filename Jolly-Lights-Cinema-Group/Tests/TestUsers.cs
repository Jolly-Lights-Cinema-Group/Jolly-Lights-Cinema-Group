using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public static class TestUsers
    {
        public static void CreateTestUsers()
        {
            DatabaseManager.InitializeDatabase();
            EmployeeService employeeService = new EmployeeService();

            Employee Employee = new Employee("Employee", "Test", "01-01-1111", "Testway 32", "test.employee@email.com", "test_employee", "testemployee", Role.Employee);
            Employee Admin = new Employee("Admin", "Test", "01-01-1111", "Testway 32", "test.admin@email.com", "test_admin", "testadmin", Role.Admin);
            Employee Manager = new Employee("Manager", "Test", "01-01-1111", "Testway 32", "test.manager@email.com", "test_manager", "testmanager", Role.Manager);

            employeeService.RegisterEmployee(Employee);
            employeeService.RegisterEmployee(Admin);
            employeeService.RegisterEmployee(Manager);

            Console.WriteLine("Test users added successfully.");
        }
    }
}
