using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class CustomerOrderRepository
    {
        public void AddCustomerOrder(double grandPrice)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO CustomerOrder (GrandPrice)
                    VALUES (@grandPrice);";

                command.Parameters.AddWithValue("@grandPrice", grandPrice);

                command.ExecuteNonQuery();
                Console.WriteLine("Customer order added successfully.");
            }
        }

        public List<string> GetAllCustomerOrders()
        {
            var customerOrders = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, GrandPrice FROM CustomerOrder;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerOrders.Add($"ID: {reader.GetInt32(0)}, Grand total: {reader.GetString(1)}");
                    }
                }
            }
            return customerOrders;
        }
    }
}
