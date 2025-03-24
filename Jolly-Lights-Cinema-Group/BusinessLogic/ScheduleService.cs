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
    public void RegisterSchedule(int movieRoomId, int movieId, DateTime startDate)
    {
        _scheduleRepo.AddSchedule(movieRoomId, movieId, startDate);
    }
}

// Should we add a check for checking if id's are valid? I think so, but this can be added later