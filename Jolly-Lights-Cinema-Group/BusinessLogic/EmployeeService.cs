using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

// Business Service that will validate Userinput. For now it won't do much, except that it will Verify user registration (RegisterEmployee) input. 
//

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepo;

    public EmployeeService()
    {
        _employeeRepo = new EmployeeRepository();
    }

    public void RegisterEmployee(string firstName, string lastName, string email, string username, string password, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return;
        }

        if (!email.Contains("@"))
        {
            Console.WriteLine("Error: Invalid email format.");
            return;
        }

        _employeeRepo.AddEmployee(firstName, lastName, email, username, password, (int)role);
    }

    public void ShowAllEmployees()
    {
        List<string> employees = _employeeRepo.GetAllEmployees();
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
        }
        else
        {
            Console.WriteLine("Employees:");
            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }
        }
    }

        public bool VerifyLogin(string userName, string password)
        {
            return _employeeRepo.VerifyLogin(userName, password);
        }
}
