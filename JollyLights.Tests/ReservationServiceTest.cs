using Xunit;
using Moq;
using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

public class ReservationServiceTests
{
    [Fact]
    public void TestGetReservedSeats()
    {
        Mock<ScheduleSeatRepository> mockScheduleSeatRepo = new Mock<ScheduleSeatRepository>();
        Schedule dummySchedule = new Schedule(1, 1, 1, DateTime.Today, new TimeSpan(14, 0, 0));
        
        mockScheduleSeatRepo
            .Setup(repo => repo.GetSeatsBySchedule(1))
            .Returns(new List<ScheduleSeat>
            {
                new ScheduleSeat (1, 1, 10.0, SeatType.RegularSeat, "1,16"),
                new ScheduleSeat (1, 1, 10.0, SeatType.RegularSeat, "1,17")
            });

        ReservationService reservationService = new ReservationService(scheduleSeatRepository: mockScheduleSeatRepo.Object);

        List<(string, string)> result = reservationService.GetReservedSeats(dummySchedule);

        List<(string, string)> expected = new List<(string, string)>
        {
            ("1", "16"),
            ("1", "17")
        };

        Xunit.Assert.Equal(expected, result);
    }
}