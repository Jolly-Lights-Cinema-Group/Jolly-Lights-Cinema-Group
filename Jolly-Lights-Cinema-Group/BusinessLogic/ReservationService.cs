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

    public List<(string, string)> GetReservedSeats(int movieRoomId)
    {
        ScheduleSeatRepository scheduleSeatRepository = new();
        var result = scheduleSeatRepository.GetReservedSeats(movieRoomId);
        return result.Select(x => (x.Split(',')[0], x.Split(',')[1])).ToList();
    }
}
