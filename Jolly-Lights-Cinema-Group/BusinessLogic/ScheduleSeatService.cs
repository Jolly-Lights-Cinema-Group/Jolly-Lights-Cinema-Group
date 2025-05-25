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

        public bool RegisterScheduleSeat(ScheduleSeat scheduleSeat)
        {
            return _scheduleSeatRepo.AddScheduleSeat(scheduleSeat);
        }

        public List<ScheduleSeat> ShowAllLScheduleSeats(int scheduleId)
        {
            return _scheduleSeatRepo.GetSeatsBySchedule(scheduleId);
        }
    }
}
