using JollyLightsCinemaGroup.DataAccess;

public class ScheduleService
{
    private readonly ScheduleRepository _scheduleRepo;
    private readonly MovieRepository _movieRepository;
    private readonly MovieRoomRepository _movieRoomRepository;
    private readonly MovieRoomService _movieRoomService;

    public ScheduleService(ScheduleRepository? scheduleRepository = null, MovieRepository? movieRepository = null, MovieRoomRepository? movieRoomRepository = null, MovieRoomService? movieRoomService = null)
    {
        _scheduleRepo = scheduleRepository ?? new ScheduleRepository();
        _movieRepository = movieRepository ?? new MovieRepository();
        _movieRoomRepository = movieRoomRepository ?? new MovieRoomRepository();
        _movieRoomService = movieRoomService ?? new MovieRoomService();
    }

    public bool RegisterSchedule(Schedule schedule)
    {
        return _scheduleRepo.AddSchedule(schedule);
    }

    public bool CanAddSchedule(int roomId, DateTime startDate, TimeSpan startTime, int movieId, int movieDuration)
    {
        if (_scheduleRepo.CanAddScheduleAfter(roomId, startDate, startTime, movieDuration) && _scheduleRepo.CanAddScheduleBefore(roomId, startDate, startTime, movieId))
        {
            return true;
        }
        return false;
    }

    public bool DeleteSchedule(Schedule schedule)
    {
        return _scheduleRepo.DeleteScheduleLine(schedule);
    }

    // Show Schedule

    public List<Schedule> ShowScheduleByDate(DateTime dateTime, int locationId)
    {
        List<Schedule> schedules = _scheduleRepo.GetScheduleByDate(dateTime, locationId);
        return schedules;
    }

    public List<Movie> GetMoviesBySchedule(int locationId)
    {
        List<Schedule> schedules = _scheduleRepo.GetAllUpcomingSchedules(locationId);

        List<Movie> uniqueMovies = new();
        HashSet<int?> addedMovieIds = new();

        foreach (Schedule schedule in schedules)
        {
            if (!addedMovieIds.Contains(schedule.MovieId))
            {
                Movie? movie = _movieRepository.GetMovieById(schedule.MovieId);
                if (movie != null)
                {
                    uniqueMovies.Add(movie);
                    addedMovieIds.Add(movie.Id);
                }
            }
        }
        return uniqueMovies;
    }

    public List<IGrouping<DateTime, Schedule>> GroupedSchedules(Movie selectedMovie, int locationId)
    {
        List<Schedule> schedules = _scheduleRepo.GetSchedulesByMovie(selectedMovie);

        schedules = schedules
            .Where(s =>
                s.StartDate.Add(s.StartTime) >= DateTime.Now &&
                _movieRoomRepository.GetMovieRoomById(s.MovieRoomId)!.LocationId == locationId &&
                _movieRoomService.GetLeftOverSeats(s) > 0
            )
            .ToList();

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
