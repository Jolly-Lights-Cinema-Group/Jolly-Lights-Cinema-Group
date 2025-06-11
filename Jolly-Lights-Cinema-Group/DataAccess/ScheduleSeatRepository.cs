using Jolly_Lights_Cinema_Group.Enum;
using Microsoft.Data.Sqlite;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleSeatRepository
    {
        public virtual List<ScheduleSeat> GetSeatsBySchedule(int scheduleId)
        {
            var seats = new List<ScheduleSeat>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, ScheduleId, ReservationId, Price, Type, SeatNumber FROM ScheduleSeat WHERE ScheduleId = @ScheduleId;";
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int? reservationId = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                        ScheduleSeat seat = new ScheduleSeat(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(3), (SeatType)reader.GetInt32(4), reader.GetString(5), reservationId);
                        seats.Add(seat);
                    }
                }
            }
            return seats;
        }

        public ScheduleSeat? AddSeatToReservation(ScheduleSeat scheduleSeat)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO ScheduleSeat (SeatNumber, ReservationId, Price, Type, ScheduleId)
                    VALUES (@SeatNumber, @ReservationId, @Price, @Type, @ScheduleId);";

                command.Parameters.AddWithValue("@SeatNumber", scheduleSeat.SeatNumber);

                if (scheduleSeat.ReservationId.HasValue)
                {
                    command.Parameters.AddWithValue("@ReservationId", scheduleSeat.ReservationId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@ReservationId", DBNull.Value);
                }

                command.Parameters.AddWithValue("@Price", scheduleSeat.Price);
                command.Parameters.AddWithValue("@Type", scheduleSeat.Type);
                command.Parameters.AddWithValue("@ScheduleId", scheduleSeat.ScheduleId);
                command.ExecuteNonQuery();

                var idCommand = connection.CreateCommand();
                idCommand.CommandText = "SELECT last_insert_rowid();";

                var result = idCommand.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    scheduleSeat.Id = Convert.ToInt32(result);
                    return scheduleSeat;
                }

                return null;
            }
        }

        public List<ScheduleSeat> GetSeatsByReservation(Reservation reservation)
        {
            List<ScheduleSeat> scheduleSeats = new List<ScheduleSeat>();
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, ScheduleId, ReservationId, Price, Type, SeatNumber
                    FROM ScheduleSeat
                    WHERE ReservationId = @reservationId;";

                command.Parameters.AddWithValue("@reservationId", reservation.Id);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ScheduleSeat scheduleSeat = new ScheduleSeat(reader.GetInt32(0), reader.GetInt32(2), reader.GetDouble(3), (SeatType)reader.GetInt32(4), reader.GetString(5), reader.GetInt32(1));
                        scheduleSeats.Add(scheduleSeat);
                    }
                }
            }
            return scheduleSeats;
        }

        public void DeleteSeat(ScheduleSeat scheduleSeat)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM ScheduleSeat WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", scheduleSeat.Id!.Value);
                command.ExecuteNonQuery();
            }
        }
    }
}
