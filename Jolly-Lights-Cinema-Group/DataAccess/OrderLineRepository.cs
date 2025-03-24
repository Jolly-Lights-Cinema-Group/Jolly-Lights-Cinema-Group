using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class OrderLineRepository
    {
        public void AddOrderLine(int orderId, int quantity, string description, int vatPercentage, double price)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO OrderLine (OrderId, Quantity, Description, VatPercentage, Price)
                    VALUES (@orderId, @quantity, @description, @vatPercentage, @price);";

                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@vatPercentage", vatPercentage);
                command.Parameters.AddWithValue("@price", price);

                command.ExecuteNonQuery();
                Console.WriteLine("Order line added successfully.");
            }
        }
    }
}
