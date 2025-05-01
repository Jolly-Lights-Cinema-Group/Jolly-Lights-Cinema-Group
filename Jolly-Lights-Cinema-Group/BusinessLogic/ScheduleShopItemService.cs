using JollyLightsCinemaGroup.DataAccess;

public class ScheduleShopItemService
{
    private readonly ScheduleShopItemRepository _scheduleShopItemRepository = new ScheduleShopItemRepository();
    public void RegisterScheduleShopItem(ScheduleShopItem scheduleShopItem)
    {
        if (_scheduleShopItemRepository.AddScheduleShopItem(scheduleShopItem))
        {
            Console.WriteLine("Item successfully added to reservation");
            return;
        }

        Console.WriteLine("Item could not be added to reservation.");
        return;
    }

    public void DeleteScheduleShopItem(ScheduleShopItem scheduleShopItem)
    {
        if (_scheduleShopItemRepository.RemoveScheduleShopItem(scheduleShopItem))
        {
            Console.WriteLine("Item successfully removed from reservation.");
            return;
        }
        Console.WriteLine("No matching items found to remove.");
        return;
    }
}