using JollyLightsCinemaGroup.DataAccess;

public class ReservationService
{
    private readonly ReservationRepository _reservationRepository = new ReservationRepository();
    public Reservation? RegisterReservation(Reservation reservation)
    {
        return _reservationRepository.AddReservation(reservation);
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

    public List<Reservation> GetAllReservations()
    {
        return _reservationRepository.GetAllReservations();
    }

    public int? GetLocationIdByReservation(Reservation reservation)
    {
        ScheduleSeatRepository scheduleSeatRepository = new();
        ScheduleRepository scheduleRepository = new();
        MovieRoomRepository movieRoomRepository = new();

        List<ScheduleSeat> scheduleSeats = scheduleSeatRepository.GetSeatsByReservation(reservation);
        if (scheduleSeats.Count == 0)
            return null;

        ScheduleSeat scheduleSeat = scheduleSeats.First();

        Schedule? schedule = scheduleRepository.GetScheduleById(scheduleSeat.ScheduleId);
        if (schedule == null)
            return null;

        MovieRoom? movieRoom = movieRoomRepository.GetMovieRoomById(schedule.MovieRoomId);
        if (movieRoom == null)
            return null;

        return movieRoom.LocationId;
    }
}
