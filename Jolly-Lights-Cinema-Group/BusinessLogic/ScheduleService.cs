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
            Console.Clear();
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
        Console.Clear();
        if (_scheduleRepo.DeleteScheduleLine(schedule))
        {
            Console.WriteLine("Schedule deleted.");
        }
        else
        {
            Console.WriteLine("Schedule not deleted.");
        }
        Console.ReadKey();
    }

    // Show Schedule

    public void ShowSchedule(DateTime dateTime)
    {
        List<Schedule> schedules = _scheduleRepo.ShowSchedule(dateTime);

        if (schedules.Count == 0)
        {
            Console.WriteLine("No schedules where found.");
            return;
        }

        Console.WriteLine($"Schedule Movies on {dateTime.Date}:");
        foreach (var schedule in schedules)
        {
            Movie? movieinformation = MovieRepository.GetMovieById(schedule.MovieId);
            // Location information??
            // Movieroom information??
            Console.WriteLine($"Room: {schedule.MovieRoomId} Movie: {movieinformation.Title} Date: {schedule.StartDate.ToString("dd/mm/yyyy")} Time: {schedule.StartTime}");
        }
        return;
    }

    public List<Movie> GetMoviesBySchedule()
    {
        List<Schedule> schedules = _scheduleRepo.GetAllSchedules();

        // MovieRepository movieRepository = new();
        List<Movie> uniqueMovies = new();
        HashSet<int?> addedMovieIds = new();

        foreach (Schedule schedule in schedules)
        {
            if (!addedMovieIds.Contains(schedule.MovieId))
            {
                Movie? movie = MovieRepository.GetMovieById(schedule.MovieId);
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
}
