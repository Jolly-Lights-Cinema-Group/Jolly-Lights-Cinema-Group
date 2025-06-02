using JollyLightsCinemaGroup.DataAccess;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public class ScheduleSeatService
    {
        private readonly ScheduleSeatRepository _scheduleSeatRepo;

        public ScheduleSeatService()
        {
            _scheduleSeatRepo = new ScheduleSeatRepository();
        }

        public List<ScheduleSeat> ShowAllLScheduleSeats(int scheduleId)
        {
            return _scheduleSeatRepo.GetSeatsBySchedule(scheduleId);
        }

        public List<string> GetReservedSeatsByMovieRoom(int movieRoomId)
        {
            return _scheduleSeatRepo.GetReservedSeats(movieRoomId);
        }

        public bool AddSeatToReservation(ScheduleSeat scheduleSeat)
        {
            return _scheduleSeatRepo.AddSeatToReservation(scheduleSeat);
        }

        public List<ScheduleSeat> GetSeatsByReservation(Reservation reservation)
        {
            return _scheduleSeatRepo.GetSeatsByReservation(reservation);
        }
    }
}
