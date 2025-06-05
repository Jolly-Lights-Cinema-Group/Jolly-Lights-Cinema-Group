using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class OrderLineRepository
    {
        public OrderLine? AddOrderLine(OrderLine orderLine)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO OrderLine (ReservationId, Quantity, Description, VatPercentage, Price, CustomerOrderId)
                    VALUES (@reservationId, @quantity, @description, @vatPercentage, @price, @customerOrderId);";

                if (orderLine.ReservationId.HasValue)
                {
                    command.Parameters.AddWithValue("@reservationId", orderLine.ReservationId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@reservationId", DBNull.Value);
                }

                command.Parameters.AddWithValue("@quantity", orderLine.Quantity);
                command.Parameters.AddWithValue("@description", orderLine.Description);
                command.Parameters.AddWithValue("@vatPercentage", orderLine.VatPercentage);
                command.Parameters.AddWithValue("@price", orderLine.Price);

                if (orderLine.CustomerOrderId.HasValue)
                {
                    command.Parameters.AddWithValue("@customerOrderId", orderLine.ReservationId!.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@customerOrderId", DBNull.Value);
                }

                command.ExecuteNonQuery();

                var idCommand = connection.CreateCommand();
                idCommand.CommandText = "SELECT last_insert_rowid();";

                var result = idCommand.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    orderLine.Id = Convert.ToInt32(result);
                    return orderLine;
                }

                return null;
            }
        }

        public List<OrderLine> GetOrderLinesByReservation(Reservation reservation)
        {
            List<OrderLine> orderLines = new List<OrderLine>();
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, ReservationId, Quantity, Description, VatPercentage, Price 
                    FROM OrderLine
                    WHERE ReservationId = @reservationId;";

                command.Parameters.AddWithValue("@reservationId", reservation.Id);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderLine orderLine = new OrderLine(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(3), reader.GetInt32(4), reader.GetDouble(5), reader.GetInt32(1));
                        orderLines.Add(orderLine);
                    }
                }
            }
            return orderLines;
        }

        public bool DeleteOrderLineByReservation(Reservation reservation)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM OrderLine WHERE ReservationId = @reservationId;";

                command.Parameters.AddWithValue("@reservationId", reservation.Id);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool SetCustomerOrderIdForOrderLine(OrderLine orderLine)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE OrderLine
                    SET CustomerOrderId = @customerOrderId
                    WHERE Id = @orderLineId;";

                command.Parameters.AddWithValue("@customerOrderId", orderLine.CustomerOrderId!.Value);
                command.Parameters.AddWithValue("@orderLineId", orderLine.Id);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
