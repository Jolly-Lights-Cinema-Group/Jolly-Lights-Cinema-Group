using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public static class CustomerOrderRepository
    {
        public static bool AddCustomerOrder(CustomerOrder customerOrder)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO CustomerOrder (GrandPrice)
                    VALUES (@grandPrice);";

                command.Parameters.AddWithValue("@grandPrice", customerOrder.GrandPrice);

                command.ExecuteNonQuery();
                return true;
            }
        }

        public static List<CustomerOrder> GetAllCustomerOrders()
        {
            List<CustomerOrder> customerOrders = new List<CustomerOrder>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, GrandPrice FROM CustomerOrder;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerOrder customerOrder = new(reader.GetInt32(0));
                        customerOrders.Add(customerOrder);
                    }
                }
            }
            return customerOrders;
        }
    }
}
