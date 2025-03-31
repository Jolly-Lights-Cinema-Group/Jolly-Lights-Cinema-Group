using System;
using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public static class TestUsers
    {
        public static void CreateTestUsers()
        {
            EmployeeService employeeService = new EmployeeService();

            employeeService.RegisterEmployee("Employee", "Test", "test.employee@email.com", "test_employee", "testemployee", UserRole.Employee); // Employee
            employeeService.RegisterEmployee("Admin", "Test", "test.admin@email.com", "test_admin", "testadmin", UserRole.Admin); // Admin
            employeeService.RegisterEmployee("Manager", "Test", "test.manager@email.com", "test_manager", "testmanager", UserRole.Manager); // Manager

            Console.WriteLine("Test users added successfully.");
        }
    }
}
