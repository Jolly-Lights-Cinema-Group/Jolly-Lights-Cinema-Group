using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class DiscountCodeRepository
    {
        public void AddDiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO DiscountCode (Code, DiscountAmount, DiscountType, ExperationDate, OrderId)
                    VALUES (@code, @discountAmount, @discountType, @experationDate, @orderId);";

                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@discountAmount", discountAmount);
                command.Parameters.AddWithValue("@discountType", discountType);
                command.Parameters.AddWithValue("@experationDate", experationDate);
                command.Parameters.AddWithValue("@orderId", orderId);

                command.ExecuteNonQuery();
                Console.WriteLine("Discount code added successfully.");
            }
        }
    }
}
