using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class ScheduleSeatService
{
    private readonly ScheduleSeatRepository _scheduleSeatRepo;

    public ScheduleSeatService()
    {
        _scheduleSeatRepo = new ScheduleSeatRepository();
    }

    public void RegisterScheduleSeat(int scheduleId, int reservationId, double price, int type, string seatNumber)
    {
        _scheduleSeatRepo.AddScheduleSeat(scheduleId, reservationId, price, type, seatNumber);
    }

    public void ShowAllLScheduleSeats(int scheduleId)
    {
        List<string> seats = _scheduleSeatRepo.GetSeatsBySchedule(scheduleId);
        if (seats.Count == 0)
        {
            Console.WriteLine("No reserved seats found for this schedule.");
        }
        else
        {
            Console.WriteLine("Reserved Seats:");
            foreach (var seat in seats)
            {
                Console.WriteLine(seat);
            }
        }
    }
}
