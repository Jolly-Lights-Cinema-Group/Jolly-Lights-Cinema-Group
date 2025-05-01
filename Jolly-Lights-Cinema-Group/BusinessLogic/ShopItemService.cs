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

    public void ShowAllShopItems()
    {
        List<ShopItem> shopItems = _shopItemRepository.GetAllShopItems();
        if (shopItems.Count == 0)
        {
            Console.WriteLine("No items found in shop.");
            return;
        }

        Console.WriteLine("Shop items:");
        foreach (var shopItem in shopItems)
        {
            Console.WriteLine($"Id: {shopItem.Id}; Name: {shopItem.Name}; Price: ${shopItem.Price}; Stock: {shopItem.Stock};" +
                            (shopItem.MinimumAge > 0 ? $" Minimum age: {shopItem.MinimumAge}" : ""));
        }
        return;
    }
    public void UpdateShopItem(ShopItem shopItem, string? newName, string? newPrice, string? newStock, string? newMinimumAge)
    {
        if (_shopItemRepository.ModifyShopItem(shopItem, newName, newPrice, newStock, newMinimumAge))
        {
            Console.WriteLine("Shop item is updated");
            return;
        }
        Console.WriteLine("No item found in shop to update.");
        return;
    }
}
