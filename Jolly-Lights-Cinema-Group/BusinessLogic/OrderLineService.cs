using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

public class OrderLineService
{
    private readonly OrderLineRepository _orderLineRepo;
    private readonly ScheduleSeatRepository _scheduleSeatRepository;
    private readonly ScheduleShopItemRepository _scheduleShopItemRepository;
    private readonly ShopItemRepository _shopItemRepository;

    public OrderLineService(
        OrderLineRepository? orderLineRepository = null,
        ScheduleSeatRepository? scheduleSeatRepository = null,
        ScheduleShopItemRepository? scheduleShopItemRepository = null,
        ShopItemRepository? shopItemRepository = null)
    {
        _orderLineRepo = orderLineRepository ?? new OrderLineRepository();
        _scheduleSeatRepository = scheduleSeatRepository ?? new ScheduleSeatRepository();
        _scheduleShopItemRepository = scheduleShopItemRepository ?? new ScheduleShopItemRepository();
        _shopItemRepository = shopItemRepository ?? new ShopItemRepository();
    }

    public bool DeleteOrderLineByReservation(Reservation reservation)
    {
        return _orderLineRepo.DeleteOrderLineByReservation(reservation);
    }
    public void CreateOrderLineForReservation(Reservation reservation)
    {
        CreateOrderLineForScheduleShopItem(reservation);
        CreateOrderLineForScheduleSeats(reservation);
    }

    public List<OrderLine> CreateOrderLineForScheduleSeats(Reservation reservation)
    {
        List<ScheduleSeat> scheduleSeats = _scheduleSeatRepository.GetSeatsByReservation(reservation);
        return CreateOrderLineForCashDeskTickets(scheduleSeats, reservation);
    }

    public List<OrderLine> CreateOrderLineForScheduleShopItem(Reservation reservation)
    {
        List<ShopItem> shopItems = new List<ShopItem>();
        List<ScheduleShopItem> scheduleShopItems = _scheduleShopItemRepository.GetScheduleShopItemByReservation(reservation);

        foreach (ScheduleShopItem scheduleShopItem in scheduleShopItems)
        {
            ShopItem? shopItem = _shopItemRepository.GetShopItemById(scheduleShopItem.ShopItemId);
            if (shopItem != null) shopItems.Add(shopItem);
        }

        return CreateOrderLineForCashDeskShopItems(shopItems, reservation);
    }

    public List<OrderLine> GetOrderLinesByReservation(Reservation reservation)
    {
        return _orderLineRepo.GetOrderLinesByReservation(reservation);
    }

    public List<OrderLine> CreateOrderLineForCashDeskShopItems(List<ShopItem> shopItems, Reservation? reservation = null)
    {
        List<OrderLine> orderLines = new List<OrderLine>();

        shopItems.RemoveAll(item => item.Id is null);

        List<IGrouping<int, ShopItem>> groupedItems = shopItems
            .GroupBy(item => item.Id!.Value)
            .ToList();

        foreach (IGrouping<int, ShopItem> group in groupedItems)
        {
            int shopItemId = group.Key;
            int quantity = group.Count();

            ShopItem shopItem = _shopItemRepository.GetShopItemById(shopItemId)!;

            double totalPrice = quantity * shopItem.Price;

            OrderLine orderLine = new OrderLine(quantity, shopItem.Name, shopItem.VatPercentage, Math.Round(totalPrice, 2));
            if (reservation != null) orderLine.ReservationId = (int)reservation.Id!;

            OrderLine? orderLineId = _orderLineRepo.AddOrderLine(orderLine);

            if (orderLineId != null)
            {
                orderLines.Add(orderLineId);
            }
        }
        return orderLines;
    }

    public List<OrderLine> CreateOrderLineForCashDeskTickets(List<ScheduleSeat> scheduleSeats, Reservation? reservation = null)
    {
        List<OrderLine> orderLines = new List<OrderLine>();

        List<IGrouping<SeatType, ScheduleSeat>> groupedSeats = scheduleSeats
            .GroupBy(seat => seat.Type)
            .ToList();

        foreach (var group in groupedSeats)
        {
            SeatType seatType = group.Key;
            int quantity = group.Count();
            double totalPrice = group.Sum(seat => (double)seat.Price);

            OrderLine orderLine = new OrderLine(quantity, seatType.ToString(), 9, Math.Round(totalPrice, 2));
            if (reservation != null) orderLine.ReservationId = (int)reservation.Id!;

            OrderLine? orderLineId = _orderLineRepo.AddOrderLine(orderLine);

            if (orderLineId != null)
            {
                orderLines.Add(orderLineId);
            }
        }
        return orderLines;
    }

    public bool ConnectCustomerOrderIdToOrderLine(List<OrderLine> orderLines)
    {
        foreach (OrderLine orderLine in orderLines)
        {
            if (!_orderLineRepo.SetCustomerOrderIdForOrderLine(orderLine))
            {
                return false;
            }
        }

        return true;
    }
}
