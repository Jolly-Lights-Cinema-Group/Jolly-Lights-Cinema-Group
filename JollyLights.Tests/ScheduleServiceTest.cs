using Xunit;
using Moq;
using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

public class ScheduleServiceTests
{
    private readonly Mock<ScheduleRepository> _mockScheduleRepo = new Mock<ScheduleRepository>();
    private readonly Mock<MovieRepository> _mockMovieRepo = new Mock<MovieRepository>();
    private readonly Mock<MovieRoomRepository> _mockMovieRoomRepo = new Mock<MovieRoomRepository>();
    private readonly Mock<ScheduleSeatRepository> _mockScheduleSeatRepo = new Mock<ScheduleSeatRepository>();
    private readonly Mock<MovieRoomService> _mockMovieRoomService;

    private readonly ScheduleService _scheduleService;

    public ScheduleServiceTests()
    {
        _mockMovieRoomService = new Mock<MovieRoomService>(_mockMovieRoomRepo.Object, _mockScheduleSeatRepo.Object);
        _scheduleService = new ScheduleService(_mockScheduleRepo.Object, _mockMovieRepo.Object, _mockMovieRoomRepo.Object, _mockMovieRoomService.Object);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void TestCanAddSchedule(bool canAddAfter, bool canAddBefore, bool expected)
    {
        _mockScheduleRepo.Setup(repo => repo.CanAddScheduleAfter(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>()))
                         .Returns(canAddAfter);
        _mockScheduleRepo.Setup(repo => repo.CanAddScheduleBefore(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>()))
                         .Returns(canAddBefore);

        bool result = _scheduleService.CanAddSchedule(1, DateTime.Today, TimeSpan.FromHours(15), 10, 120);

        Xunit.Assert.Equal(expected, result);
    }

    [Fact]
    public void TestGetMoviesBySchedule()
    {
        int locationId = 1;
        List<Schedule> schedules = new List<Schedule>
        {
            new Schedule(1, 1, 1, DateTime.Today, new TimeSpan(09, 0, 0)),
            new Schedule(2, 1, 2, DateTime.Today, new TimeSpan(12, 0, 0)),
            new Schedule(3, 1, 1, DateTime.Today, new TimeSpan(18, 0, 0))
        };

        Movie movie1 = new Movie(1, "Barbie", 45, 6, DateTime.Today, "Margot Robbie, Ryan Gossling");
        Movie movie2 = new Movie(2, "Final Destination", 120, 16, DateTime.Today, "");

        _mockScheduleRepo.Setup(repo => repo.GetAllUpcomingSchedules(locationId)).Returns(schedules);
        _mockMovieRepo.Setup(repo => repo.GetMovieById(1)).Returns(movie1);
        _mockMovieRepo.Setup(repo => repo.GetMovieById(2)).Returns(movie2);

        List<Movie> result = _scheduleService.GetMoviesBySchedule(locationId);

        Xunit.Assert.Equal(2, result.Count);
        Xunit.Assert.Contains(result, movie => movie.Id == 1);
        Xunit.Assert.Contains(result, movie => movie.Id == 2);
    }

    [Fact]
    public void TestGroupedSchedules()
    {
        Movie selectedMovie = new Movie(1, "Hans en Grietje", 45, 6, DateTime.Today, "Hans, Grietje, Heks");;
        int locationId = 10;

        DateTime today = DateTime.Today;

        List<Schedule> schedules = new List<Schedule>
        {
            new Schedule(1, 1, 1, today.AddDays(1), TimeSpan.FromHours(14)),
            new Schedule(2, 1, 1, today.AddDays(2), TimeSpan.FromHours(16)),
            new Schedule(3, 1, 1, today.AddDays(-1), TimeSpan.FromHours(12))
        };

        string json = "[[\"S\",\"P\",\"V\"],[\"_\",\"S\",\"#\"]]";
        MovieRoom movieRoom = new MovieRoom(1, 1, json, MovieType.DolbyImax, locationId);

        _mockScheduleRepo.Setup(repo => repo.GetSchedulesByMovie(selectedMovie)).Returns(schedules);
        _mockMovieRoomRepo.Setup(repo => repo.GetMovieRoomById(It.IsAny<int>())).Returns(movieRoom);

        _mockMovieRoomService.Setup(service => service.GetLeftOverSeats(It.IsAny<Schedule>())).Returns(5);

        var result = _scheduleService.GroupedSchedules(selectedMovie, locationId);

        Xunit.Assert.Equal(2, result.Count);
        foreach (var group in result)
        {
            Xunit.Assert.True(group.Key >= today);
        }
    }
}