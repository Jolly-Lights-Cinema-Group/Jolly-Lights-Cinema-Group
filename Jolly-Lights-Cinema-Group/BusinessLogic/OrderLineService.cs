using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class OrderLineService
{
    private readonly OrderLineRepository _orderLineRepo = new OrderLineRepository();

    public void RegisterOrderLine(OrderLine orderLine)
    {
        _orderLineRepo.AddOrderLine(orderLine);
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

            OrderLine orderLine = new OrderLine((int)reservation.Id!, quantity, seatType.ToString(), 21, totalPrice);

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
            int vatPercentage = 21;

            OrderLine orderLine = new OrderLine((int)reservation.Id!, quantity, shopItem.Name, vatPercentage, totalPrice);

            _orderLineRepo.AddOrderLine(orderLine);
        }
    }
}
