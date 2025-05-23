using Jolly_Lights_Cinema_Group;
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

    public bool RegisterSchedule(Schedule schedule)
    {
        return _scheduleRepo.AddSchedule(schedule);
    }

    public bool CanAddSchedule(int roomId, DateTime startDate, TimeSpan startTime)
    {
        return _scheduleRepo.CanAddSchedule(roomId, startDate, startTime);
    }

    public bool DeleteSchedule(Schedule schedule)
    {
        return _scheduleRepo.DeleteScheduleLine(schedule);
    }

    // Show Schedule

    public List<Schedule> ShowScheduleByDate(DateTime dateTime)
    {
        List<Schedule> schedules = _scheduleRepo.ShowSchedule(dateTime);
        return schedules;
    }

    public List<Movie> GetMoviesBySchedule()
    {
        List<Schedule> schedules = _scheduleRepo.GetAllSchedules();

        MovieRepository movieRepository = new();

        // MovieRepository movieRepository = new();
        List<Movie> uniqueMovies = new();
        HashSet<int?> addedMovieIds = new();

        foreach (Schedule schedule in schedules)
        {
            if (!addedMovieIds.Contains(schedule.MovieId))
            {
                Movie? movie = movieRepository.GetMovieById(schedule.MovieId);
                if (movie != null)
                {
                    uniqueMovies.Add(movie);
                    addedMovieIds.Add(movie.Id);
                }
            }
        }
        return uniqueMovies;
    }

    public List<IGrouping<DateTime, Schedule>> GroupedSchedules(Movie selectedMovie)
    {
        List<Schedule> schedules = _scheduleRepo.GetSchedulesByMovie(selectedMovie);

        var groupedSchedules = schedules
            .GroupBy(s => s.StartDate.Date)
            .OrderBy(g => g.Key)
            .ToList();

        return groupedSchedules;
    }

    public bool UpdateFreeTimeColumn()
    {
        return _scheduleRepo.UpdateFreeTimeColumn();
    }

    public List<Schedule> GetSchedulesByMovie(Movie movie)
    {
        return _scheduleRepo.GetSchedulesByMovie(movie);
    }

    public List<Schedule> GetSchedulesByMovieAndRoom(Movie movie, MovieRoom movieRoom)
    {
        return _scheduleRepo.GetScheduleByMovieAndRoom(movie, movieRoom);
    }
}
