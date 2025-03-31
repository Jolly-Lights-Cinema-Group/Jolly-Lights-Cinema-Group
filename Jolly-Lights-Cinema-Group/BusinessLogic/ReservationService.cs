using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class ReservationService
{
    private readonly ReservationRepository _reservationRepo;

    public ReservationService()
    {
        _reservationRepo = new ReservationRepository();
    }

    public void RegisterReservation(string reservationNumber, int orderId, int paid)
    {
        if (string.IsNullOrWhiteSpace(reservationNumber))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return;
        }
        if (int.IsNegative(paid))
        {
            Console.WriteLine("Error: Paid cannot be an negative number.");
            return;
        }

        _reservationRepo.AddReservation(reservationNumber, orderId, paid);
    }

    public void ShowAllReservations()
    {
        List<string> reservations = _reservationRepo.GetAllReservations();
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
        }
        else
        {
            Console.WriteLine("Reservations:");
            foreach (var reservation in reservations)
            {
                Console.WriteLine(reservation);
            }
        }
    }
}
