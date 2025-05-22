using JollyLightsCinemaGroup.DataAccess;

public class ScheduleShopItemService
{
    private readonly ScheduleShopItemRepository _scheduleShopItemRepository = new ScheduleShopItemRepository();
    public bool RegisterScheduleShopItem(ScheduleShopItem scheduleShopItem)
    {
        if (_scheduleShopItemRepository.AddScheduleShopItem(scheduleShopItem))
        {
            return true;
        }

        return false; ;
    }

    public bool DeleteScheduleShopItem(ShopItem shopItem, Reservation reservation)
    {
        return _scheduleShopItemRepository.RemoveScheduleShopItem(shopItem, reservation);
    }

    public List<ScheduleShopItem> GetScheduleShopItemByReservation(Reservation reservation)
    {
        return _scheduleShopItemRepository.GetScheduleShopItemByReservation(reservation);
    }
}