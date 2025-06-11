using Xunit;
using Moq;
using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

public class OrderLineServiceTests
{
    private readonly Mock<OrderLineRepository> _orderLineRepoMock = new();
    private readonly Mock<ScheduleSeatRepository> _scheduleSeatRepoMock = new();
    private readonly Mock<ScheduleShopItemRepository> _scheduleShopItemRepoMock = new();
    private readonly Mock<ShopItemRepository> _shopItemRepoMock = new();
    private readonly OrderLineService _orderLineService;

    public OrderLineServiceTests()
    {
        _orderLineService = new OrderLineService(_orderLineRepoMock.Object, _scheduleSeatRepoMock.Object, _scheduleShopItemRepoMock.Object, _shopItemRepoMock.Object);
    }

    [Fact]
    public void TestCreateOrderLineForScheduleSeats()
    {
        Reservation reservation = new Reservation(1, "John", "Doe", 068112347, "john.doe@gmail.com", "x", false);
        List<ScheduleSeat> seats = new List<ScheduleSeat>
        {
            new ScheduleSeat(1, 1, 10.0, SeatType.RegularSeat, "0,1"),
            new ScheduleSeat(2, 1, 20.0, SeatType.RegularSeat, "0,2"),
            new ScheduleSeat(3, 1, 15.0, SeatType.LoveSeat, "0,3")
        };
        _scheduleSeatRepoMock.Setup(repo => repo.GetSeatsByReservation(reservation)).Returns(seats);
        _orderLineRepoMock.Setup(repo => repo.AddOrderLine(It.IsAny<OrderLine>())).Returns((OrderLine orderLine) => orderLine);

        List<OrderLine> result = _orderLineService.CreateOrderLineForScheduleSeats(reservation);

        Xunit.Assert.Equal(2, result.Count);

        OrderLine? regularOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == SeatType.RegularSeat.ToString());
        Xunit.Assert.NotNull(regularOrderLine);
        Xunit.Assert.Equal(2, regularOrderLine.Quantity);
        Xunit.Assert.Equal(9, regularOrderLine.VatPercentage);
        Xunit.Assert.Equal(Math.Round(10.0 + 20.0, 2), regularOrderLine.Price);
        Xunit.Assert.Equal(reservation.Id, regularOrderLine.ReservationId);

        OrderLine? loveOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == SeatType.LoveSeat.ToString());
        Xunit.Assert.NotNull(loveOrderLine);
        Xunit.Assert.Equal(1, loveOrderLine.Quantity);
        Xunit.Assert.Equal(9, loveOrderLine.VatPercentage);
        Xunit.Assert.Equal(15, loveOrderLine.Price);
        Xunit.Assert.Equal(reservation.Id, loveOrderLine.ReservationId);
    }

    [Fact]
    public void TestCreateOrderLineForScheduleShopItem()
    {
        Reservation reservation = new Reservation(1, "Jane", "Doe", 068112347, "john.doe@gmail.com", "x", false);
        List<ScheduleShopItem> scheduleShopItems = new List<ScheduleShopItem>
        {
            new ScheduleShopItem(1, 1, reservation.Id!.Value),
            new ScheduleShopItem(2, 2, reservation.Id.Value),
            new ScheduleShopItem(3, 2, reservation.Id.Value)
        };

        _scheduleShopItemRepoMock.Setup(repo => repo.GetScheduleShopItemByReservation(reservation)).Returns(scheduleShopItems);

        _shopItemRepoMock
            .Setup(repo => repo.GetShopItemById(1))
            .Returns(new ShopItem(1, "Popcorn S", 2.0, 10, 1, 9));
        _shopItemRepoMock
            .Setup(repo => repo.GetShopItemById(2))
            .Returns(new ShopItem(2, "Popcorn M", 4.0, 10, 1, 9));

        _orderLineRepoMock.Setup(repo => repo.AddOrderLine(It.IsAny<OrderLine>())).Returns((OrderLine orderLine) => orderLine);

        List<OrderLine> result = _orderLineService.CreateOrderLineForScheduleShopItem(reservation);

        Xunit.Assert.Equal(2, result.Count);

        OrderLine? popCornSOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == "Popcorn S");
        Xunit.Assert.NotNull(popCornSOrderLine);
        Xunit.Assert.Equal(1, popCornSOrderLine.Quantity);
        Xunit.Assert.Equal(9, popCornSOrderLine.VatPercentage);
        Xunit.Assert.Equal(2, popCornSOrderLine.Price);
        Xunit.Assert.Equal(reservation.Id, popCornSOrderLine.ReservationId);

        OrderLine? popCornMOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == "Popcorn M");
        Xunit.Assert.NotNull(popCornMOrderLine);
        Xunit.Assert.Equal(2, popCornMOrderLine.Quantity);
        Xunit.Assert.Equal(9, popCornMOrderLine.VatPercentage);
        Xunit.Assert.Equal(8, popCornMOrderLine.Price);
        Xunit.Assert.Equal(reservation.Id, popCornMOrderLine.ReservationId);
    }
}
