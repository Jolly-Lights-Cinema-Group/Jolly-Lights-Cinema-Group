using System;
using System.Collections.Generic;
using Jolly_Lights_Cinema_Group.Enum;
using Microsoft.Data.Sqlite;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleSeatRepository
    {
        public bool AddScheduleSeat(ScheduleSeat scheduleSeat)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO ScheduleSeat (ScheduleId, ReservationId, Price, Type, SeatNumber)
                    VALUES (@scheduleId, @reservationId, @price, @type, @seatNumber);";

                command.Parameters.AddWithValue("@scheduleId", scheduleSeat.ScheduleId);
                command.Parameters.AddWithValue("@reservationId", scheduleSeat.ReservationId);
                command.Parameters.AddWithValue("@price", scheduleSeat.Price);
                command.Parameters.AddWithValue("@type", scheduleSeat.Type);
                command.Parameters.AddWithValue("@seatNumber", scheduleSeat.SeatNumber);

                return command.ExecuteNonQuery() > 0;
            }
        }

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

        public List<string> GetReservedSeats(int roomNumber, int locationId)
        {
            var result = new List<string>();
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SeatNumber
                    FROM ScheduleSeat
                    LEFT JOIN Schedule ON ScheduleSeat.ScheduleId = Schedule.Id
                    WHERE Schedule.MovieRoomId = @RoomNumber;";

                command.Parameters.AddWithValue("@RoomNumber", roomNumber);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            return result;
        }
        
        public void AddSeatToReservation(string seat, SeatType type, int reservationId, int scheduleId, double price)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO ScheduleSeat (SeatNumber, ReservationId, Price, Type, ScheduleId)
                    VALUES (@SeatNumber, @ReservationId, @Price, @Type, @ScheduleId);";

                command.Parameters.AddWithValue("@SeatNumber", seat);
                command.Parameters.AddWithValue("@ReservationId", reservationId);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                command.ExecuteNonQuery();
            }
        }
    }
}
