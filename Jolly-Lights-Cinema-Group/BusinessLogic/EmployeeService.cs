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
        return _employeeRepo.AddEmployee(employee);
    }

    public bool DeleteEmployee(Employee employee)
    {

        return _employeeRepo.DeleteEmployee(employee);
    }

    public List<Employee> ShowAllEmployees()
    {
        return _employeeRepo.GetAllEmployees();
    }

    public void ChangeFirstName(string firstname, string username)
    {
        if (_employeeRepo.ChangeFirstNameDB(firstname, username))
        {
            Console.WriteLine("Firstname changed.");
        }
        else
        {
            Console.WriteLine("Firstname didn't changed.");
        }

    }

    public void ChangeLastName(string lastname, string username)
    {
        if (_employeeRepo.ChangeLastNameDB(lastname, username))
        {
            Console.WriteLine("Lastname changed.");
        }
        else
        {
            Console.WriteLine("Lastname didn't changed.");
        }
    }

    public void ChangeEmail(string email, string username)
    {
        if (_employeeRepo.ChangeEmailDB(email, username))
        {
            Console.WriteLine("email changed.");
        }
        else
        {
            Console.WriteLine("email didn't changed.");
        }
    }

    public void ChangePassword(string password, string username)
    {
        if (_employeeRepo.ChangePasswordDB(password, username))
        {
            Console.WriteLine("password changed.");
        }
        else
        {
            Console.WriteLine("password didn't changed.");
        }
    }

    public bool UserNameExists(string userName)
    {
        return _employeeRepo.UserNameAlreadyExist(userName);
    }

    public Employee? GetEmployeeByUserName(string userName, string firstName, string lastName)
    {
        return _employeeRepo.GetEmployeeByUsername(userName, firstName, lastName);
    }
}
