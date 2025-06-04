using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class OrderLineRepository
    {
        public bool AddOrderLine(OrderLine orderLine)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO OrderLine (ReservationId, Quantity, Description, VatPercentage, Price)
                    VALUES (@reservationId, @quantity, @description, @vatPercentage, @price);";

                command.Parameters.AddWithValue("@reservationId", orderLine.ReservationId);
                command.Parameters.AddWithValue("@quantity", orderLine.Quantity);
                command.Parameters.AddWithValue("@description", orderLine.Description);
                command.Parameters.AddWithValue("@vatPercentage", orderLine.VatPercentage);
                command.Parameters.AddWithValue("@price", orderLine.Price);

                return command.ExecuteNonQuery() > 0;
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
    }
}
