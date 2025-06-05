using JollyLightsCinemaGroup.DataAccess;

public class ShopItemService
{
    private readonly ShopItemRepository _shopItemRepository = new ShopItemRepository();
    public bool RegisterShopItem(ShopItem shopItem)
    {
        return _shopItemRepository.AddShopItem(shopItem);
    }

    public List<ShopItem> GetAllShopItems()
    {
        List<ShopItem> shopItems = _shopItemRepository.GetAllShopItems();
        return shopItems;
    }
    public List<ShopItem> GetAllShopItems(int locationId)
    {
        List<ShopItem> shopItems = _shopItemRepository.GetAllShopItems(locationId);
        return shopItems;
    }

    public bool UpdateShopItem(ShopItem shopItem, string? newName, string? newPrice, string? newStock, string? newMinimumAge)
    {
        return _shopItemRepository.ModifyShopItem(shopItem, newName, newPrice, newStock, newMinimumAge);
    }

    public bool SellShopItem(ShopItem shopItem, Reservation reservation)
    {
        if (shopItem.Id != null && reservation.Id != null)
        {
            ScheduleShopItem scheduleShopItem = new(shopItem.Id.Value, reservation.Id.Value);
            ScheduleShopItemRepository scheduleShopItemRepository = new();

            if (SellShopItem(shopItem) == true && scheduleShopItemRepository.AddScheduleShopItem(scheduleShopItem) == true)
            {
                return true;
            }
        }
        return false;
    }

    public bool SellShopItem(ShopItem shopItem)
    {
        return _shopItemRepository.ModifyShopItem(shopItem, "", "", Convert.ToString(shopItem.Stock -= 1), "");
    }

    public ShopItem? GetShopItemById(int id)
    {
        return _shopItemRepository.GetShopItemById(id);
    }

    public bool RestoreShopItem(ShopItem shopItem, int quantity = 1)
    {
        return _shopItemRepository.ModifyShopItem(shopItem, "", "", Convert.ToString(shopItem.Stock += quantity), "");
    }
}
