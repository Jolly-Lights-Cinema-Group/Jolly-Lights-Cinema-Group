using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

public class OrderLineService
{
    private readonly OrderLineRepository _orderLineRepo = new OrderLineRepository();

    public void RegisterOrderLine(OrderLine orderLine)
    {
        _orderLineRepo.AddOrderLine(orderLine);
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

    public void CreateOrderLineForScheduleSeats(Reservation reservation)
    {
        ScheduleSeatRepository scheduleSeatRepository = new();
        SeatRepository seatRepository = new();

        List<ScheduleSeat> scheduleSeats = scheduleSeatRepository.GetSeatsByReservation(reservation);

        List<IGrouping<SeatType, ScheduleSeat>> groupedSeats = scheduleSeats
            .GroupBy(seat => seat.Type)
            .ToList();

        foreach (var group in groupedSeats)
        {
            SeatType seatType = group.Key;
            int quantity = group.Count();
            double totalPrice = group.Sum(seat => (double)seat.Price);

            OrderLine orderLine = new OrderLine(quantity, seatType.ToString(), 9, totalPrice, (int)reservation.Id!);

            _orderLineRepo.AddOrderLine(orderLine);
        }
    }

    public void CreateOrderLineForScheduleShopItem(Reservation reservation)
    {
        ScheduleShopItemRepository scheduleShopItemRepository = new();
        ShopItemRepository shopItemRepository = new();

        List<ScheduleShopItem> scheduleShopItems = scheduleShopItemRepository.GetScheduleShopItemByReservation(reservation);

        List<IGrouping<int, ScheduleShopItem>> groupedItems = scheduleShopItems
            .GroupBy(item => item.ShopItemId)
            .ToList();

        foreach (IGrouping<int, ScheduleShopItem> group in groupedItems)
        {
            int shopItemId = group.Key;
            int quantity = group.Count();

            ShopItem shopItem = shopItemRepository.GetShopItemById(shopItemId)!;
            if (shopItem == null)
                continue;

            double totalPrice = quantity * shopItem.Price;

            OrderLine orderLine = new OrderLine(quantity, shopItem.Name, shopItem.VatPercentage, totalPrice, (int)reservation.Id!);

            _orderLineRepo.AddOrderLine(orderLine);
        }
    }

    public List<OrderLine> GetOrderLinesByReservation(Reservation reservation)
    {
        return _orderLineRepo.GetOrderLinesByReservation(reservation);
    }

    public List<OrderLine> CreateOrderLineForScheduleShopItem(Reservation reservation, List<ShopItem> shopItems)
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

            ShopItemRepository shopItemRepository = new();

            ShopItem shopItem = shopItemRepository.GetShopItemById(shopItemId)!;
            if (shopItem == null)
                continue;

            double totalPrice = quantity * shopItem.Price;

            OrderLine orderLine = new OrderLine(quantity, shopItem.Name, shopItem.VatPercentage, totalPrice, (int)reservation.Id!);

            _orderLineRepo.AddOrderLine(orderLine);
            orderLines.Add(orderLine);
        }
        return orderLines;
    }
}
