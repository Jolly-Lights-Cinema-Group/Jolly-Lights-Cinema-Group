using Jolly_Lights_Cinema_Group;

public class ShopMenu
{
    private readonly ShopItemService _shopitemService;

    public ShopMenu()
    {
        _shopitemService = new ShopItemService();
    }

    public void DisplayShop(Reservation reservation)
    {
        var shopItems = _shopitemService.GetAllShopItems();

        bool inShop = true;

        string[] menuItems = shopItems
            .Where(item => item.Stock > 0)
            .Select(item => $"{item.Name}: €{Math.Round(item.Price * (1.0 + ((double)item.VatPercentage / 100)), 2)}")
            .Append("Finish")
            .ToArray();
        
        Menu shopMenu = new("Shop", menuItems);

        while (inShop)
        {
            int choice = shopMenu.Run();
            if (choice == shopItems.Count)
            {
                inShop = false;
                continue;
            }

            if (choice >= 0 && choice < shopItems.Count)
            {
                ShopItem selectedItem = shopItems[choice];
                if (selectedItem.Stock <= 0)
                {
                    Console.WriteLine($"{selectedItem.Name} out of stock.");
                    Console.WriteLine("\nPress any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                
                if (_shopitemService.SellShopItem(selectedItem, reservation))
                {
                    Console.WriteLine($"{selectedItem.Name} added to reservation: {reservation.ReservationNumber}.");
                }
                else
                {
                    Console.WriteLine($"{selectedItem.Name} could not be added to the reservation.");
                }
            }

            else
            {
                Console.WriteLine("Invalid choice.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}