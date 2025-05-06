using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ShopHandler
    {
        private static List<ShopItem> _shopItems = new();
        public static void ManageShop()
        {
            ShopItemRepository shopItemRepository = new ShopItemRepository();
            _shopItems = shopItemRepository.GetAllShopItems();

            string[] menuItems = _shopItems
                .Select(item => $"{item.Name}: €{item.Price}")
                .Append("Finish")
                .ToArray();
            
            Menu shopMenu = new("Shop", menuItems);

            bool inShop = true;

            while (inShop)
            {
                int choice = shopMenu.Run();
                inShop = HandleShopChoice(choice);
                Console.Clear();
            }
        }

        private static bool HandleShopChoice(int choice)
        {
            if (choice == _shopItems.Count)
            {
                return false;
            }

            if (choice >= 0 && choice < _shopItems.Count)
            {
                ShopItem selectedItem = _shopItems[choice];
                Console.WriteLine($"{selectedItem.Name} added.");
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
