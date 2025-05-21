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

    public bool DeleteReservation(Reservation reservation)
    {
        if (_reservationRepository.RemoveReservation(reservation))
        {
            return true;
        }
        return false;
    }

    public Reservation? FindReservationByReservationNumber(string reservationNumber)
    {
        Reservation? reservation = _reservationRepository.FindReservationByReservationNumber(reservationNumber);
        if (reservation is null) return null;

        return reservation;
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
