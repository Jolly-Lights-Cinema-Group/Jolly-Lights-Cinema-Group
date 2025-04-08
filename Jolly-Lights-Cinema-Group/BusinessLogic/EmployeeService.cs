using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;
using Jolly_Lights_Cinema_Group.Enum;
using Microsoft.VisualBasic;
using Jolly_Lights_Cinema_Group.Models;
using Jolly_Lights_Cinema_Group.Common;

// Business Service that will validate Userinput. For now it won't do much, except that it will Verify user registration (RegisterEmployee) input. 
//

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepo;

    public EmployeeService()
    {
        _employeeRepo = new EmployeeRepository();
    }

    public bool RegisterEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FirstName) || string.IsNullOrWhiteSpace(employee.LastName))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return false;
        }

        if (!employee.Email.Contains("@"))
        {
            Console.WriteLine("Error: Invalid email format.");
            return false;
        }

        if (!Enum.IsDefined(typeof(Role), employee.Role))
        {
            Console.WriteLine("Error: Invalid role.");
            return false;
        }

        if (_employeeRepo.UserNameAlreadyExist(employee.UserName))
        {
            Console.WriteLine("Error: Username already exists.");
            return false;
        }

        _employeeRepo.AddEmployee(employee);
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

    public void ChangeFirstName(string username, string firstname)
    {
        if (_employeeRepo.ChangeFirstNameDB(username,firstname))
        {
            Console.WriteLine("Firstname changed.");
        }
        else
        {
            Console.WriteLine("Firstname didn't changed.");
        }

    }

    public void ChangeLastName(string username, string lastname)
    {
        if (_employeeRepo.ChangeLastNameDB(username,lastname))
        {
            Console.WriteLine("Firstname changed.");
        }
        else
        {
            Console.WriteLine("Firstname didn't changed.");
        }
    }

    public void ChangeEmail(string email)
    {}

    public void ChangePassword(string password)
    {}
}
