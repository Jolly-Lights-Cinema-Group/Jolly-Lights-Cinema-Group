using JollyLightsCinemaGroup.DataAccess;

public class ShopItemService
{
    private readonly ShopItemRepository _shopItemRepository = new ShopItemRepository();
    public void RegisterShopItem(ShopItem shopItem)
    {
        if (_shopItemRepository.AddShopItem(shopItem))
        {
            Console.WriteLine("Item successfully added to the shop.");
            return;
        }

        Console.WriteLine("Item was not added to the shop.");
        return;
    }

    public List<ShopItem> GetAllShopItems()
    {
        List<ShopItem> shopItems = _shopItemRepository.GetAllShopItems();
        return shopItems;
    }
    public void UpdateShopItem(ShopItem shopItem, string? newName, string? newPrice, string? newStock, string? newMinimumAge)
    {
        if (_shopItemRepository.ModifyShopItem(shopItem, newName, newPrice, newStock, newMinimumAge))
        {
            Console.WriteLine($"{shopItem.Name} is updated");
            return;
        }
        Console.WriteLine("No item found in shop to update.");
        return;
    }

    public bool SellShopItem(ShopItem shopItem, Reservation reservation)
    {
        if (shopItem.Id != null && reservation.Id != null)
        {
            ScheduleShopItem scheduleShopItem = new(shopItem.Id.Value, reservation.Id.Value);
            ScheduleShopItemRepository scheduleShopItemRepository = new();

            if (_shopItemRepository.ModifyShopItem(shopItem, "", "", Convert.ToString(shopItem.Stock -= 1), "") == true && scheduleShopItemRepository.AddScheduleShopItem(scheduleShopItem) == true)
            {
                return true;
            }
        }
        return false;
    }

    public ShopItem? GetShopItemById(int id)
    {
        return _shopItemRepository.GetShopItemById(id);
    }

    public bool RestoreShopItem(ShopItem shopItem)
    {
        return _shopItemRepository.ModifyShopItem(shopItem, "", "", Convert.ToString(shopItem.Stock += 1), "");
    }
}
