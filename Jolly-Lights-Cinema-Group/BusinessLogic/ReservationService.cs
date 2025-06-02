using JollyLightsCinemaGroup.DataAccess;

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
        return _reservationRepository.IsReservationPaid(reservation);
    }

    public List<(string, string)> GetReservedSeats(Schedule schedule)
    {
        ScheduleSeatRepository scheduleSeatRepository = new();
        List<ScheduleSeat> result = scheduleSeatRepository.GetSeatsBySchedule(schedule.Id!.Value);

        List<(string, string)> reservedSeats = new List<(string, string)>();

        foreach (var seat in result)
        {
            var parts = seat.SeatNumber!.Split(',');
            var row = parts[0].Trim();
            var col = parts[1].Trim();

            reservedSeats.Add((row, col));
        }

        return reservedSeats;
    }
}
