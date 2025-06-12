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

    [Fact]
    public void TestCreateOrderLineForCashDeskShopItem()
    {
        List<ShopItem> shopItems = new List<ShopItem>
        {
            new ShopItem(1, "Cola", 2.0, 10, 1, 9),
            new ShopItem(1, "Cola", 2.0, 10, 1, 9),
            new ShopItem(2, "Cava", 10.0, 10, 1, 21),
            new ShopItem(2, "Cava", 10.0, 10, 1, 21)
        };

        _shopItemRepoMock
            .Setup(repo => repo.GetShopItemById(1))
            .Returns(new ShopItem(1, "Cola", 2.0, 10, 1, 9));
        _shopItemRepoMock
            .Setup(repo => repo.GetShopItemById(2))
            .Returns(new ShopItem(2, "Cava", 10.0, 10, 1, 21));

        _orderLineRepoMock.Setup(repo => repo.AddOrderLine(It.IsAny<OrderLine>())).Returns((OrderLine orderLine) => orderLine);

        List<OrderLine> result = _orderLineService.CreateOrderLineForCashDeskShopItems(shopItems);

        Xunit.Assert.Equal(2, result.Count);

        OrderLine? colaOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == "Cola");
        Xunit.Assert.NotNull(colaOrderLine);
        Xunit.Assert.Equal(2, colaOrderLine.Quantity);
        Xunit.Assert.Equal(9, colaOrderLine.VatPercentage);
        Xunit.Assert.Equal(4, colaOrderLine.Price);
        Xunit.Assert.Null(colaOrderLine.ReservationId);

        OrderLine? cavaOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == "Cava");
        Xunit.Assert.NotNull(cavaOrderLine);
        Xunit.Assert.Equal(2, cavaOrderLine.Quantity);
        Xunit.Assert.Equal(21, cavaOrderLine.VatPercentage);
        Xunit.Assert.Equal(20, cavaOrderLine.Price);
        Xunit.Assert.Null(cavaOrderLine.ReservationId);
    }

    [Fact]
    public void TestCreateOrderLineForCashDeskTickets()
    {
        List<ScheduleSeat> seats = new List<ScheduleSeat>
        {
            new ScheduleSeat(1, 1, 10.0, SeatType.RegularSeat, "0,1"),
            new ScheduleSeat(2, 1, 20.0, SeatType.VipSeat, "0,2"),
            new ScheduleSeat(3, 1, 15.0, SeatType.LoveSeat, "0,3")
        };

        _orderLineRepoMock.Setup(repo => repo.AddOrderLine(It.IsAny<OrderLine>())).Returns((OrderLine orderLine) => orderLine);
        List<OrderLine> result = _orderLineService.CreateOrderLineForCashDeskTickets(seats);

        Xunit.Assert.Equal(3, result.Count);

        OrderLine? vipOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == SeatType.VipSeat.ToString());
        Xunit.Assert.NotNull(vipOrderLine);
        Xunit.Assert.Equal(1, vipOrderLine.Quantity);
        Xunit.Assert.Equal(9, vipOrderLine.VatPercentage);
        Xunit.Assert.Equal(20, vipOrderLine.Price);
        Xunit.Assert.Null(vipOrderLine.ReservationId);

        OrderLine? regularOrderLine = result.FirstOrDefault(orderLine => orderLine.Description == SeatType.RegularSeat.ToString());
        Xunit.Assert.NotNull(regularOrderLine);
        Xunit.Assert.Equal(1, regularOrderLine.Quantity);
        Xunit.Assert.Equal(9, regularOrderLine.VatPercentage);
        Xunit.Assert.Equal(10, regularOrderLine.Price);
        Xunit.Assert.Null(regularOrderLine.ReservationId);
    }
}
