using Jolly_Lights_Cinema_Group.Enum;
using Microsoft.Data.Sqlite;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleSeatRepository
    {
        public List<ScheduleSeat> GetSeatsBySchedule(int scheduleId)
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
                        ScheduleSeat seat = new ScheduleSeat(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDouble(3), (SeatType)reader.GetInt32(4), reader.GetString(5));
                        seats.Add(seat);
                    }
                }
            }
            return seats;
        }

        public bool AddSeatToReservation(ScheduleSeat scheduleSeat)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO ScheduleSeat (SeatNumber, ReservationId, Price, Type, ScheduleId)
                    VALUES (@SeatNumber, @ReservationId, @Price, @Type, @ScheduleId);";

                command.Parameters.AddWithValue("@SeatNumber", scheduleSeat.SeatNumber);
                command.Parameters.AddWithValue("@ReservationId", scheduleSeat.ReservationId);
                command.Parameters.AddWithValue("@Price", scheduleSeat.Price);
                command.Parameters.AddWithValue("@Type", scheduleSeat.Type);
                command.Parameters.AddWithValue("@ScheduleId", scheduleSeat.ScheduleId);
                return command.ExecuteNonQuery() > 0;
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
                        ScheduleSeat scheduleSeat = new ScheduleSeat(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDouble(3), (SeatType)reader.GetInt32(4), reader.GetString(5));
                        scheduleSeats.Add(scheduleSeat);
                    }
                }
            }
            return scheduleSeats;
        }
    }
}
