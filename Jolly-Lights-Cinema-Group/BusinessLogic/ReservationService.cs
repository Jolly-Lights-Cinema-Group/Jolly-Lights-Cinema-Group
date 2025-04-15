using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public static class ReservationService
{
    public static void RegisterReservation(Reservation reservation)
    {
        if (ReservationRepository.AddReservation(reservation))
        {
            Console.WriteLine("Reservation added successfully.");
            return;
        }

        Console.WriteLine("Reservation was not added to the database.");
        return;
    }

    public static void DeleteReservation(Reservation reservation)
    {
        if (ReservationRepository.RemoveReservation(reservation))
        {
            Console.WriteLine("Reservation removed successfully.");
            return;
        }
        Console.WriteLine("No matching reservation found to remove.");
        return;
    }

    public static void ShowAllReservations()
    {
        List<Reservation> reservations = ReservationRepository.GetAllReservations();
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
            return;
        }
        Console.WriteLine("Reservations:");
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine($"Reservation Number: {reservation.ReservationNumber}; Name: {reservation.FirstName} {reservation.LastName}; Phone Number{reservation.PhoneNumber}; EMail: {reservation.EMail}; Paid: {reservation.Paid}; Order Id: {reservation.OrderId}");
        }
    }
}
