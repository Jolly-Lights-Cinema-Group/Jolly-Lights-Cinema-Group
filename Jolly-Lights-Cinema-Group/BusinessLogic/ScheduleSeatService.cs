using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public class ScheduleSeatService
    {
        private readonly ScheduleSeatRepository _scheduleSeatRepo;

        public ScheduleSeatService()
        {
            _scheduleSeatRepo = new ScheduleSeatRepository();
        }

        public void RegisterScheduleSeat(ScheduleSeat scheduleSeat)
        {
            if (_scheduleSeatRepo.AddScheduleSeat(scheduleSeat))
            {
                Console.WriteLine($"Seat: {scheduleSeat.SeatNumber} reserved.");
                return;
            }
        }

        public void ShowAllLScheduleSeats(int scheduleId)
        {
            List<ScheduleSeat> seats = _scheduleSeatRepo.GetSeatsBySchedule(scheduleId);
            if (seats.Count == 0)
            {
                Console.WriteLine("No reserved seats found for this schedule.");
            }
            else
            {
                Console.WriteLine("Reserved Seats:");
                foreach (var seat in seats)
                {
                    Console.WriteLine($"Seatnumber: {seat.SeatNumber}; Seat type: {seat.Type}; Price: €{seat.Price}");
                }
            }
        }
    }
}
