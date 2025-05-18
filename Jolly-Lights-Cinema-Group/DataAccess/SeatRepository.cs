using Jolly_Lights_Cinema_Group.Enum;
using Jolly_Lights_Cinema_Group.Models;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class SeatRepository
    {
        public List<Seat> GetSeatPrices(int locationId)
        {
            List<Seat> seats = new List<Seat>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Type, Price FROM Seats WHERE LocationId = @LocationId;";
                command.Parameters.AddWithValue("@LocationId", locationId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var seat = new Seat
                        {
                            Id = reader.GetInt32(0),
                            Type = (SeatType)reader.GetInt32(1),
                            Price = reader.GetDouble(2)
                        };
                        seats.Add(seat);
                    }
                }
            }
            return seats;
        }

        public double GetSeatPriceForSeatTypeOnLocation(SeatType type, int locationId)
        {
            var price = 0d;
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Price FROM Seats WHERE LocationId = @LocationId AND Type = @Type LIMIT 1;";
                command.Parameters.AddWithValue("@LocationId", locationId);
                command.Parameters.AddWithValue("@Type", type);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        price = reader.GetDouble(0);
                    }
                }
            }
            return price;
        }

        public void ModifySeatPrices(Seat seat, decimal newPrice, int locationId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Seats  SET Price = @NewPrice WHERE Id = @SeatId AND LocationId = @LocationId;";
                command.Parameters.AddWithValue("@NewPrice", newPrice);
                command.Parameters.AddWithValue("@SeatId", seat.Id);
                command.Parameters.AddWithValue("@LocationId", locationId);

                command.ExecuteNonQuery();
            }
        }
    }
}
