using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Win32.SafeHandles;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class EmployeeRepository
    {
        public void AddEmployee(Employee employee)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Employee (FirstName, LastName, DateOfBirth, Address, Email, UserName, Password, Role)
                    VALUES (@firstName, @lastName, @dateofbirth, @address, @email, @username, @password, @role);";

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password);

                command.Parameters.AddWithValue("@firstName", employee.FirstName);
                command.Parameters.AddWithValue("@lastName", employee.LastName);
                command.Parameters.AddWithValue("@dateofbirth", employee.DateofBirth);
                command.Parameters.AddWithValue("@address", employee.Address);
                command.Parameters.AddWithValue("@email", employee.Email);
                command.Parameters.AddWithValue("@username", employee.UserName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@role", employee.Role);

                command.ExecuteNonQuery();
            }
        }

        public bool DeleteEmployee(string firstname, string lastname)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Employee WHERE FirstName = @firstName AND LastName = @lastName";

                command.Parameters.AddWithValue("@firstName", firstname);
                command.Parameters.AddWithValue("@lastName", lastname);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UserNameAlreadyExist(string username)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE username = @username";

                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = Convert.ToInt32(command.ExecuteScalar());
                return rowsAffected > 0;
            }
        }

        public List<string> GetAllEmployees()
        {
            var employees = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, FirstName, LastName, Email FROM Employee;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add($"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)} {reader.GetString(2)}, Email: {reader.GetString(3)}");
                    }
                }
            }
            return employees;
        }

        public bool ChangeFirstNameDB(string username, string firstname)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE EMPLOYEE SET firstname = @firstname WHERE USERNAME = @username";

                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool ChangeLastNameDB(string username, string lastname)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE EMPLOYEE SET lastname = @lastname WHERE USERNAME = @username";

                command.Parameters.AddWithValue("@lastname", lastname);
                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public void ChangeEmailDB(string username, string email)
        { }

        public void ChangePasswordDB(string username, string password)
        { }


    }
}
