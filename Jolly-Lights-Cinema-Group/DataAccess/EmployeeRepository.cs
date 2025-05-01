using System;
using System.Collections.Generic;
using System.Data;
using Jolly_Lights_Cinema_Group.Enum;
using Microsoft.Data.Sqlite;
using Microsoft.Win32.SafeHandles;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class EmployeeRepository
    {
        public bool AddEmployee(Employee employee)
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

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
        }

        public bool DeleteEmployee(Employee employee)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Employee WHERE FirstName = @firstName AND LastName = @lastName";

                command.Parameters.AddWithValue("@firstName", employee.FirstName);
                command.Parameters.AddWithValue("@lastName", employee.LastName);

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

        public Employee? GetEmployeeByUsername(Employee employee)  // not implemented yet. Only used for tests
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT FirstName, LastName, DateOfBirth, Address, Email, UserName, Password, Role
                FROM Employee
                WHERE UserName = @username;";

                command.Parameters.AddWithValue("@username", employee.UserName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employee(
                            reader.GetString(0), // FirstName
                            reader.GetString(1), // LastName
                            reader.GetString(2), // DateOfBirth
                            reader.GetString(3), // Address
                            reader.GetString(4), // Email
                            reader.GetString(5), // Username
                            reader.GetString(6), // Password
                            (Role)Enum.Parse(typeof(Role), reader.GetString(7)) // Role
                        );
                    }
                }
            }

            return null;
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT FirstName, LastName, DateOfBirth, Address, Email, UserName, Password, Role FROM Employee;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // This is a small filter function to check if the Column is null or not.
                        // Reader will throw errors if the column value is null.
                        string SafeGet(int index) => reader.IsDBNull(index) ? "" : reader.GetString(index);

                        employees.Add(new Employee(
                            SafeGet(0),
                            SafeGet(1),
                            SafeGet(2),
                            SafeGet(3),
                            SafeGet(4),
                            SafeGet(5),
                            SafeGet(6),
                            Enum.TryParse(SafeGet(7), out Role role) ? role : Role.Employee
                        ));
                    }
                }
            }
            return employees;
        }

        public bool ChangeFirstNameDB(string firstname, string username)
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

        public bool ChangeLastNameDB(string lastName, string username)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE EMPLOYEE SET lastname = @lastname WHERE USERNAME = @username";

                command.Parameters.AddWithValue("@lastname", lastName);
                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool ChangeEmailDB(string email, string username)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE EMPLOYEE SET email = @email WHERE USERNAME = @username";

                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool ChangePasswordDB(string password, string username)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE EMPLOYEE SET Password = @password WHERE USERNAME = @username";

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
