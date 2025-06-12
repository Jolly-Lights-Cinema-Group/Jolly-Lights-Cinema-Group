using Xunit;
using Moq;
using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

public class MovieRoomServiceTests
{
    [Fact]
    public void TestGetLeftOverSeats()
    {
        Mock<MovieRoomRepository> mockMovieRoomRepo = new Mock<MovieRoomRepository>();
        Mock<ScheduleSeatRepository> mockScheduleSeatRepo = new Mock<ScheduleSeatRepository>();

        int roomId = 1;
        int scheduleId = 1;

        var json = "[[\"S\",\"P\",\"V\"],[\"_\",\"S\",\"#\"]]";
        mockMovieRoomRepo.Setup(repo => repo.GetRoomLayoutJson(roomId)).Returns(json);

        List<ScheduleSeat> reservedSeats = new List<ScheduleSeat>
        {
            new ScheduleSeat(1, scheduleId, 10.0, SeatType.RegularSeat, "0,1"),
            new ScheduleSeat(2, scheduleId, 20.0, SeatType.VipSeat, "0,2"),
            new ScheduleSeat(3, scheduleId, 15.0, SeatType.LoveSeat, "0,3")
        };

        mockScheduleSeatRepo.Setup(repo => repo.GetSeatsBySchedule(scheduleId)).Returns(reservedSeats);

        MovieRoomService movieRoomService = new MovieRoomService(mockMovieRoomRepo.Object, mockScheduleSeatRepo.Object);

        Schedule schedule = new Schedule(scheduleId, roomId, 1, DateTime.Today, new TimeSpan(12, 0, 0));

        int leftOver = movieRoomService.GetLeftOverSeats(schedule);

        Xunit.Assert.Equal(1, leftOver);
    }
}
