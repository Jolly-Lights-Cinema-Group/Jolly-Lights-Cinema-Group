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
        List<OrderLine> orderLines = new List<OrderLine>();
        List<ScheduleSeat> scheduleSeats = _scheduleSeatRepository.GetSeatsByReservation(reservation);

        List<IGrouping<SeatType, ScheduleSeat>> groupedSeats = scheduleSeats
            .GroupBy(seat => seat.Type)
            .ToList();

        foreach (var group in groupedSeats)
        {
            SeatType seatType = group.Key;
            int quantity = group.Count();
            double totalPrice = group.Sum(seat => (double)seat.Price);

            OrderLine orderLine = new OrderLine(quantity, seatType.ToString(), 9, Math.Round(totalPrice, 2), (int)reservation.Id!);

            _orderLineRepo.AddOrderLine(orderLine);
            orderLines.Add(orderLine);
        }

        return orderLines;
    }

    public List<OrderLine> CreateOrderLineForScheduleShopItem(Reservation reservation)
    {
        List<OrderLine> orderLines = new List<OrderLine>();
        List<ScheduleShopItem> scheduleShopItems = _scheduleShopItemRepository.GetScheduleShopItemByReservation(reservation);

        List<IGrouping<int, ScheduleShopItem>> groupedItems = scheduleShopItems
            .GroupBy(item => item.ShopItemId)
            .ToList();

        foreach (IGrouping<int, ScheduleShopItem> group in groupedItems)
        {
            int shopItemId = group.Key;
            int quantity = group.Count();

            ShopItem shopItem = _shopItemRepository.GetShopItemById(shopItemId)!;
            if (shopItem == null)
                continue;

            double totalPrice = quantity * shopItem.Price;

            OrderLine orderLine = new OrderLine(quantity, shopItem.Name, shopItem.VatPercentage, Math.Round(totalPrice, 2), (int)reservation.Id!);

            _orderLineRepo.AddOrderLine(orderLine);
            orderLines.Add(orderLine);
        }

        return orderLines;
    }

    public List<OrderLine> GetOrderLinesByReservation(Reservation reservation)
    {
        return _orderLineRepo.GetOrderLinesByReservation(reservation);
    }

    public List<OrderLine> CreateOrderLineForCashDeskShopItems(List<ShopItem> shopItems)
    {
        List<OrderLine> orderLines = new List<OrderLine>();

        for (int i = 0; i < shopItems.Count; i++)
        {
            ShopItem item = shopItems[i];
            if (item.Id is null)
            {
                shopItems.Remove(item);
            }
        }

        List<IGrouping<int, ShopItem>> groupedItems = shopItems
            .Where(item => item.Id.HasValue)
            .GroupBy(item => item.Id!.Value)
            .ToList();

        foreach (IGrouping<int, ShopItem> group in groupedItems)
        {
            int shopItemId = group.Key;
            int quantity = group.Count();

            ShopItem shopItem = _shopItemRepository.GetShopItemById(shopItemId)!;
            if (shopItem == null)
                continue;

            double totalPrice = quantity * shopItem.Price;

            OrderLine orderLine = new OrderLine(quantity, shopItem.Name, shopItem.VatPercentage, Math.Round(totalPrice, 2));
            OrderLine? orderLineId = _orderLineRepo.AddOrderLine(orderLine);

            if (orderLineId != null)
            {
                orderLines.Add(orderLineId);
            }
        }
        return orderLines;
    }

    public List<OrderLine> CreateOrderLineForCashDeskTickets(List<ScheduleSeat> scheduleSeats)
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
