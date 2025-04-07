using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;
using Jolly_Lights_Cinema_Group.Enum;

// Business Service that will validate Userinput. For now it won't do much, except that it will Verify user registration (RegisterEmployee) input. 
//

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepo;

    public EmployeeService()
    {
        _employeeRepo = new EmployeeRepository();
    }

    public bool RegisterEmployee(string firstName, string lastName, string email, string username, string password, Role role)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return false;
        }

        if (!email.Contains("@"))
        {
            Console.WriteLine("Error: Invalid email format.");
            return false;
        }

        if (!Enum.IsDefined(typeof(Role), role))
        {
            Console.WriteLine("Error: Invalid role.");
            return false;
        }

        _employeeRepo.AddEmployee(firstName, lastName, email, username, password, (int)role);
        return true;
     }

      public void DeleteEmployee(string firstName,string lastname)
      {
        
        if (_employeeRepo.DeleteEmployee(firstName,lastname))
        {Console.WriteLine("Employee Deleted Successfully");}
        else
        {
            Console.WriteLine("Employee not found.");
        }
        
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
