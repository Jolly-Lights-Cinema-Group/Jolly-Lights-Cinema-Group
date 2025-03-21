using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

    // Manager for the Employee Table. For now, it has the functions: AddEmployee, GetAllEmployees and VerifyLogin (password).
    // To do: 
    // - Adding new methods;
    // - For security sake: Hash passwords (didn't have time to get that working for now).

namespace JollyLightsCinemaGroup.DataAccess
{
    public class EmployeeRepository
    {
        public void AddEmployee(string firstName, string lastName, string email, string username, string password)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Employee (FirstName, LastName, Email, UserName, Password)
                    VALUES (@firstName, @lastName, @email, @username, @password);";

                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password); // Change here for hashed password

                command.ExecuteNonQuery();
                Console.WriteLine("Employee added successfully.");
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

        public bool VerifyLogin(string userName, string password)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Employee WHERE UserName = @UserName AND Password = @Password";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }
    }
}
