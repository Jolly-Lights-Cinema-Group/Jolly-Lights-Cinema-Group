using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleSeatRepository
    {
        public void AddScheduleSeat(int scheduleId, int reservationId, double price, int type, string seatNumber)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO ScheduleSeat (ScheduleId, ReservationId, Price, Type, SeatNumber)
                    VALUES (@scheduleId, @reservationId, @price, @type, @seatNumber);";

                command.Parameters.AddWithValue("@scheduleId", scheduleId);
                command.Parameters.AddWithValue("@reservationId", reservationId);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@seatNumber", seatNumber);

                command.ExecuteNonQuery();
                Console.WriteLine("Schedule seat added successfully.");
            }
        }

        public List<string> GetSeatsBySchedule(int scheduleId)
        {
            var seats = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Price, Type, SeatNumber FROM ScheduleSeat WHERE ScheduleId = @ScheduleId;";
                command.Parameters.AddWithValue("@ScheduleId", scheduleId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        seats.Add($"ID: {reader.GetInt32(0)}, Price: {reader.GetString(1)}, Type: {reader.GetString(2)}, Seat number: {reader.GetString(3)}");
                    }
                }
            }
            return seats;
        }
    }
}
