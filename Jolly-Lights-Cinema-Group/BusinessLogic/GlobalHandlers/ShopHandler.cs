using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ShopHandler
    {
        private static List<ShopItem> _shopItems = new();
        public static void ManageShop(Reservation reservation)
        {
            ShopItemRepository shopItemRepository = new ShopItemRepository();
            _shopItems = shopItemRepository.GetAllShopItems();

            string[] menuItems = _shopItems
                .Select(item => $"{item.Name}: €{Math.Round(item.Price * (1.0 + ((double)item.VatPercentage / 100)), 2)}")
                .Append("Finish")
                .ToArray();
            
            Menu shopMenu = new("Shop", menuItems);

            bool inShop = true;

            while (inShop)
            {
                int choice = shopMenu.Run();
                inShop = HandleShopChoice(choice, reservation);
                Console.Clear();
            }
        }

        private static bool HandleShopChoice(int choice, Reservation reservation)
        {
            if (choice == _shopItems.Count)
            {
                return false;
            }

            if (choice >= 0 && choice < _shopItems.Count)
            {
                ShopItem selectedItem = _shopItems[choice];
                ShopItemService shopItemService = new();
                shopItemService.SellShopItem(selectedItem, reservation);
            }

            else
            {
                Console.WriteLine("Invalid choice.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            return true;
        }
    }
}
