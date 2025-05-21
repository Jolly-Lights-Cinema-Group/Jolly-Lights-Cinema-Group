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

    public bool DeleteScheduleShopItem(ScheduleShopItem scheduleShopItem)
    {
        return _scheduleShopItemRepository.RemoveScheduleShopItem(scheduleShopItem);
    }

    public List<ScheduleShopItem> GetScheduleShopItemByReservation(Reservation reservation)
    {
        return _scheduleShopItemRepository.GetScheduleShopItemByReservation(reservation);
    }
}