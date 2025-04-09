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

    public void ChangeEmail(string lastname, string username)
    { }

    public void ChangePassword(string lastname, string username)
    { }
}
