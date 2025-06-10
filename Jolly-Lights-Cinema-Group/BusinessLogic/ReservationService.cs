using JollyLightsCinemaGroup.DataAccess;

public class ReservationService
{
    private readonly ReservationRepository _reservationRepository;
    private readonly ScheduleSeatRepository _scheduleSeatRepository;
    private readonly ScheduleRepository _scheduleRepository;
    private readonly MovieRoomRepository _movieRoomRepository;

    public ReservationService(
        ReservationRepository? reservationRepository = null,
        ScheduleSeatRepository? scheduleSeatRepository = null,
        ScheduleRepository? scheduleRepository = null,
        MovieRoomRepository? movieRoomRepository = null)
    {
        _reservationRepository = reservationRepository ?? new ReservationRepository();
        _scheduleSeatRepository = scheduleSeatRepository ?? new ScheduleSeatRepository();
        _scheduleRepository = scheduleRepository ?? new ScheduleRepository();
        _movieRoomRepository = movieRoomRepository ?? new MovieRoomRepository();
    }
    public Reservation? RegisterReservation(Reservation reservation)
    {
        return _reservationRepository.AddReservation(reservation);
    }

    public bool DeleteReservation(Reservation reservation)
    {
        return _reservationRepository.RemoveReservation(reservation);
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
        List<ScheduleSeat> result = _scheduleSeatRepository.GetSeatsBySchedule(schedule.Id!.Value);

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
        List<ScheduleSeat> scheduleSeats = _scheduleSeatRepository.GetSeatsByReservation(reservation);
        if (scheduleSeats.Count == 0)
            return null;

        ScheduleSeat scheduleSeat = scheduleSeats.First();

        Schedule? schedule = _scheduleRepository.GetScheduleById(scheduleSeat.ScheduleId);
        if (schedule == null)
            return null;

        MovieRoom? movieRoom = _movieRoomRepository.GetMovieRoomById(schedule.MovieRoomId);
        if (movieRoom == null)
            return null;

        return movieRoom.LocationId;
    }
}
