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

        public List<ScheduleSeat> AddSeatToReservation(List<ScheduleSeat> scheduleSeats)
        {
            List<ScheduleSeat> seats = new List<ScheduleSeat>();
            foreach (ScheduleSeat scheduleSeat in scheduleSeats)
            {
                ScheduleSeat? scheduleSeatId = _scheduleSeatRepo.AddSeatToReservation(scheduleSeat);
                if (scheduleSeatId != null)
                {
                    seats.Add(scheduleSeatId);
                }
            }
            return seats;
        }

        public void DeleteSeat(List<ScheduleSeat> scheduleSeats)
        {
            foreach (ScheduleSeat scheduleSeat in scheduleSeats)
            {
                _scheduleSeatRepo.DeleteSeat(scheduleSeat);
            }
        }
    }
}
