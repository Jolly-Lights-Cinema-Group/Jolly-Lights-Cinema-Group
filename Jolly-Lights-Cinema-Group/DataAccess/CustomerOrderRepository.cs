using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class CustomerOrderRepository
    {
        public bool AddCustomerOrder(CustomerOrder customerOrder)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO CustomerOrder (GrandPrice, PayDate)
                    VALUES (@grandPrice, @payDate);";

                command.Parameters.AddWithValue("@grandPrice", customerOrder.GrandPrice);
                command.Parameters.AddWithValue("@payDate", customerOrder.PayDate);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<CustomerOrder> GetAllCustomerOrders()
        {
            List<CustomerOrder> customerOrders = new List<CustomerOrder>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, GrandPrice, Paydate FROM CustomerOrder;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerOrder customerOrder = new(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2));
                        customerOrders.Add(customerOrder);
                    }
                }
            }
            return customerOrders;
        }

        public List<int> GetAvailableYears()
        {
            List<int> years = new();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT DISTINCT strftime('%Y', PayDate) as Year 
                    FROM CustomerOrder 
                    ORDER BY Year DESC;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        years.Add(int.Parse(reader.GetString(0)));
                    }
                }
            }

            return years;
        }

        public List<CustomerOrder> GetCustomerOrdersByYear(int year)
        {
            List<CustomerOrder> orders = new();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, GrandPrice, PayDate 
                    FROM CustomerOrder
                    WHERE strftime('%Y', PayDate) = @year;";

                command.Parameters.AddWithValue("@year", year.ToString());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerOrder customerOrder = new CustomerOrder(reader.GetInt32(0), reader.GetDouble(1), reader.GetDateTime(2));
                        orders.Add(customerOrder);
                    }
                }
            }

            return orders;
        }
    }
}
