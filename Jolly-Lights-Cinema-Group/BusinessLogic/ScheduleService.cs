using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class ScheduleService
{
    private readonly ScheduleRepository _scheduleRepo;

    public ScheduleService()
    {
        _scheduleRepo = new ScheduleRepository();
    }

    // Adding Schedule. Also checks if time is not overlapping other times of movies. 
    // Cleaning time is not yet included.
    public void RegisterSchedule(Schedule schedule)
    {
        bool canAdd = _scheduleRepo.CanAddSchedule(
            schedule.MovieRoomId,
            schedule.StartDate,
            schedule.StartTime
        );

        if (!canAdd)
        {
            Console.WriteLine("Schedule overlaps with another movie in the same room.");
            return;
        }

        bool scheduleAdded = _scheduleRepo.AddSchedule(schedule);
        if (scheduleAdded)
        {
            Console.WriteLine("Movie schedule successfully added!");
            _scheduleRepo.UpdateFreeTimeColumn();
        }
        else
        {
            Console.WriteLine("Something went wrong while adding the schedule.");
        }
    }


    // Deleting (based on movieid and startdate)
    public void DeleteSchedule(Schedule schedule)
    {
        Console.WriteLine();
    }

    // Show Schedule

    public void ShowSchedule(DateTime dateTime)
    {
        Console.WriteLine();
    }
}

// Should we add a check for checking if id's are valid? I think so, but this can be added later

// 