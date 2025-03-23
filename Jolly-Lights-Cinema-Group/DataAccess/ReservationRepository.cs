using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;  

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ReservationRepository
    {
        public void AddReservation(string reservationNumber, int orderId, int paid)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Reservation (ReservationNumber, OrderId, Paid)
                    VALUES (@reservationNumber, @orderId, @paid);";

                command.Parameters.AddWithValue("@reservationNumber", reservationNumber);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@paid", paid);

                command.ExecuteNonQuery();
                Console.WriteLine("Reservation added successfully.");
            }
        }

        public List<string> GetAllReservations()
        {
            var reservations = new List<string>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, ReservationNumber, OrderId, Paid FROM Reservation;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add($"ID: {reader.GetInt32(0)}, Reservation number: {reader.GetString(1)}, Order ID: {reader.GetString(2)}, Paid: {Convert.ToBoolean(reader.GetInt32(3))}");
                    }
                }
            }
            return reservations;
        }
    }
}
