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

            employeeService.RegisterEmployee("Employee", "Test", "test.employee@email.com", "test_employee", "testemployee", Role.Employee); // Employee
            employeeService.RegisterEmployee("Admin", "Test", "test.admin@email.com", "test_admin", "testadmin", Role.Admin); // Admin
            employeeService.RegisterEmployee("Manager", "Test", "test.manager@email.com", "test_manager", "testmanager", Role.Manager); // Manager

            Console.WriteLine("Test users added successfully.");
        }
    }
}
