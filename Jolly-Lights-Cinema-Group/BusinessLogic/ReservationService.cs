using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class ReservationService
{
    private readonly ReservationRepository _reservationRepository = new ReservationRepository();
    public bool RegisterReservation(Reservation reservation)
    {
        if (_reservationRepository.AddReservation(reservation))
        {
            return true;
        }
        return false;
    }

    public void DeleteReservation(Reservation reservation)
    {
        if (_reservationRepository.RemoveReservation(reservation))
        {
            Console.WriteLine("Reservation removed successfully.");
            return;
        }
        Console.WriteLine("No matching reservation found to remove.");
        return;
    }

    public void ShowAllReservations()
    {
        List<Reservation> reservations = _reservationRepository.GetAllReservations();
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
            return;
        }
        Console.WriteLine("Reservations:");
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine($"Reservation Number: {reservation.ReservationNumber}; Name: {reservation.FirstName} {reservation.LastName}; Phone Number{reservation.PhoneNumber}; EMail: {reservation.EMail}; Paid: {reservation.Paid}");
        }
    }

    public void FindReservationByReservationNumber(string reservationNumber)
    {
        Reservation? reservation = _reservationRepository.FindReservationByReservationNumber(reservationNumber);
        if (reservation != null)
        {
            Console.WriteLine($"{reservationNumber}:");
            Console.WriteLine($"Name: {reservation.FirstName} {reservation.LastName}; Phone Number: {reservation.PhoneNumber}; EMail: {reservation.EMail}; Paid: {reservation.Paid}");
            return;
        }
        Console.WriteLine($"No reservation was found with reservation number: {reservationNumber}");
    }

    public bool PayReservation(Reservation reservation)
    {
        return _reservationRepository.UpdateReservationToPaid(reservation);
    }

    public bool IsReservationPaid(Reservation reservation)
    {
        if (_reservationRepository.IsReservationPaid(reservation))
        {
            Console.WriteLine($"Reservation: {reservation.ReservationNumber} has been paid");
            return true;
        }
        return false;
    }

    public List<(string, string)> GetReservedSeats(int roomNumber, int locationId)
    {
        ScheduleSeatRepository scheduleSeatRepository = new();
        var result = scheduleSeatRepository.GetReservedSeats(roomNumber, locationId);
        return result.Select(x => (x.Split(',')[0], x.Split(',')[1])).ToList();
    }

    public Reservation? MakeReservation()
    {
        MakeReservationMenu makeReservationMenu = new();
        Reservation? reservation = makeReservationMenu.MakeReservation();

        if (reservation is null) return null;

        Reservation newReservation = _reservationRepository.FindReservationByReservationNumber(reservation.ReservationNumber)!;
        return newReservation;
    }

    public void CompleteReservation(Reservation reservation, Movie selectedMovie, Schedule selectedSchedule, string selectedSeat)
    {
        MakeReservationMenu makeReservationMenu = new();
        makeReservationMenu.CompleteReservation(reservation, selectedMovie, selectedSchedule, selectedSeat);
    }
}
