using Microsoft.Data.Sqlite;
using System.Data;
namespace JollyLightsCinemaGroup.DataAccess

{
    public class CustomerOrderRepository
    {
        public CustomerOrder? AddCustomerOrder(CustomerOrder customerOrder)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO CustomerOrder (GrandPrice, PayDate, Tax)
                    VALUES (@grandPrice, @payDate, @tax);";

                command.Parameters.AddWithValue("@grandPrice", customerOrder.GrandPrice);
                command.Parameters.AddWithValue("@payDate", customerOrder.PayDate);
                command.Parameters.AddWithValue("@tax", customerOrder.Tax);

                command.ExecuteNonQuery();

                var idCommand = connection.CreateCommand();
                idCommand.CommandText = "SELECT last_insert_rowid();";

                var result = idCommand.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    customerOrder.Id = Convert.ToInt32(result);
                    return customerOrder;
                }

                return null;
            }
        }

        public List<CustomerOrder> GetAllCustomerOrders()
        {
            List<CustomerOrder> customerOrders = new List<CustomerOrder>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, GrandPrice, Paydate, Tax FROM CustomerOrder;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerOrder customerOrder = new(reader.GetInt32(0), reader.GetDouble(1), reader.GetDateTime(2), reader.GetDouble(3));
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

        public virtual List<CustomerOrder> GetCustomerOrdersByYear(int year)
        {
            List<CustomerOrder> orders = new();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, GrandPrice, PayDate, Tax 
                    FROM CustomerOrder
                    WHERE strftime('%Y', PayDate) = @year;";

                command.Parameters.AddWithValue("@year", year.ToString());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerOrder customerOrder = new CustomerOrder(reader.GetInt32(0), reader.GetDouble(1), reader.GetDateTime(2), reader.GetDouble(3));
                        orders.Add(customerOrder);
                    }
                }
            }

            return orders;
        }

        public decimal GetEarningsForYearMonth(int year, int month)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SUM(GrandPrice)
                    FROM CustomerOrder
                    WHERE strftime('%Y', PayDate) = @year
                    AND strftime('%m', PayDate) = @month;";

                command.Parameters.Add(new SqliteParameter("@year", DbType.String) { Value = year.ToString() });
                command.Parameters.Add(new SqliteParameter("@month", DbType.String) { Value = month.ToString("D2") });

                object? result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value) return 0m;

                return Convert.ToDecimal(result);
            }
        }

        public decimal GetNetEarningsForYearMonth(int year, int month)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SUM(GrandPrice - Tax)
                    FROM CustomerOrder
                    WHERE strftime('%Y', PayDate) = @year
                    AND strftime('%m', PayDate) = @month;";

                command.Parameters.Add(new SqliteParameter("@year", DbType.String) { Value = year.ToString() });
                command.Parameters.Add(new SqliteParameter("@month", DbType.String) { Value = month.ToString("D2") });

                object? result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value) return 0m;

                return Convert.ToDecimal(result);
            }
        }
    }
}
