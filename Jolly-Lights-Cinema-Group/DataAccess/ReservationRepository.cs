using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace JollyLightsCinemaGroup.DataAccess
{
    public class ReservationRepository
    {
        public bool AddReservation(Reservation reservation)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Reservation (FirstName, LastName, PhoneNumber, EMail, ReservationNumber, Paid)
                    VALUES (@firstName, @lastName, @phoneNumber, @eMail, @reservationNumber, @paid);";

                command.Parameters.AddWithValue("@firstName", reservation.FirstName);
                command.Parameters.AddWithValue("@lastName", reservation.LastName);
                command.Parameters.AddWithValue("@phoneNumber", reservation.PhoneNumber);
                command.Parameters.AddWithValue("@eMail", reservation.EMail);
                command.Parameters.AddWithValue("@reservationNumber", reservation.ReservationNumber);
                command.Parameters.AddWithValue("@paid", Convert.ToBoolean(reservation.Paid));

                return command.ExecuteNonQuery() > 0;

            }
        }

        public bool RemoveReservation(Reservation reservation)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM Reservation
                    WHERE ReservationNumber = @reservationNumber;";

                command.Parameters.AddWithValue("@reservationNumber", reservation.ReservationNumber);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, FirstName, LastName, PhoneNumber, EMail, ReservationNumber, Paid FROM Reservation;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Reservation reservation = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5), Convert.ToBoolean(reader.GetInt32(6)));
                        reservations.Add(reservation);
                    }
                }
            }
            return reservations;
        }

        public Reservation? FindReservationByReservationNumber(string reservationNumber)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, FirstName, LastName, PhoneNumber, EMail, ReservationNumber, Paid 
                    FROM Reservation
                    WHERE ReservationNumber = @reservationNumber;";

                command.Parameters.AddWithValue("@reservationNumber", reservationNumber);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Reservation reservation = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5), Convert.ToBoolean(reader.GetInt32(6)));
                        return reservation;
                    }
                }
            }
            return null;
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
    }
}
