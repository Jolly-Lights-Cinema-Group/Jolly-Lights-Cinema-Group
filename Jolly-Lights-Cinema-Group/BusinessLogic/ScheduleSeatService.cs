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

        public bool AddSeatToReservation(ScheduleSeat scheduleSeat)
        {
            return _scheduleSeatRepo.AddSeatToReservation(scheduleSeat);
        }
    }
}
