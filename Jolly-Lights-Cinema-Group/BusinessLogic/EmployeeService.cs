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

    public void DeleteEmployee(Employee employee)
    {

        if (_employeeRepo.DeleteEmployee(employee))
        { Console.WriteLine("Employee Deleted Successfully"); }
        else
        {
            Console.WriteLine("Employee not found.");
        }

    }

    public void ShowAllEmployees()
    {
        List<Employee> employees = _employeeRepo.GetAllEmployees();
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
        }
        else
        {
            Console.WriteLine("Employees:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"Firstname: {emp.FirstName} Lastname: {emp.LastName} Date of birth: {emp.DateofBirth} Adress: {emp.Address} Email: {emp.Email} Username: {emp.UserName} Password: J0llyL1ghtC1nem@R0cks!@ Role: {emp.Role}");
            }
        }
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
}
