using Jolly_Lights_Cinema_Group.Common;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ShopManagementHandler
    {
        public static void ManageShopManagement()
        {
            bool inShopManagementMenu = true;
            ShopManagementMenu shopManagementMenu = new();
            Console.Clear();
            
            while(inShopManagementMenu)
            {
            int userChoice = shopManagementMenu.Run();
            inShopManagementMenu = HandleManageShopManagementChoice(userChoice);
            Console.Clear();
            }
        }
        private static bool HandleManageShopManagementChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    AddShopItem();
                    return true;
                case 1:
                    ModifyShopItem();
                    return true;
                case 2:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }
        public static void AddShopItem()
        {
            Console.Clear();
            Console.WriteLine("Information to add item to shop:");
            ShopItem shopItem;

            string? name;
            do
            {
                Console.Write("Enter the name of the item: ");
                name = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));

            double price;
            string? inputPrice;
            do
            {
                Console.Write("Enter the price of the item (€): ");
                inputPrice = Console.ReadLine();
            } while (!double.TryParse(inputPrice, out price) || price < 0);

            int stock;
            string? inputStock;
            do
            {
                Console.Write("Enter the stock: ");
                inputStock = Console.ReadLine();
            } while (!Int32.TryParse(inputStock, out stock) || stock < 0);

            string? inputMinimumAge;
            do
            {
                Console.Write("Enter the minimum age (leave empty for all ages): ");
                inputMinimumAge = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputMinimumAge))
                {
                    break;
                }
            } while (!int.TryParse(inputMinimumAge, out int parsedAge) || parsedAge < 0);

            if (!string.IsNullOrWhiteSpace(inputMinimumAge))
            {
                int minimumAge = int.Parse(inputMinimumAge);
                shopItem = new(name, price, stock, Globals.SessionLocationId, minimumAge);
            }
            else
            {
                shopItem = new(name, price, stock, Globals.SessionLocationId);
            }

            ShopItemService shopItemService = new ShopItemService();
            shopItemService.RegisterShopItem(shopItem);

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        public static void ModifyShopItem()
        {
            Console.Clear();

            ShopItemRepository shopItemRepository = new ShopItemRepository();
            List<ShopItem> shopItems = shopItemRepository.GetAllShopItems();

            string[] menuItems = shopItems
                .Select(item => $"Name: {item.Name}; Price: €{item.Price}; Stock: {item.Stock}; Minimum age: {item.MinimumAge}")
                .Append("Finish")
                .ToArray();
            
            Menu shopMenu = new("Select shop item to edit:", menuItems);
            int choice = shopMenu.Run();

            if (choice >= shopItems.Count) return;

            ShopItem selectedItem = shopItems[choice];

            Console.Clear();
            Console.WriteLine($"Editing: {selectedItem.Name}");

            Console.Write("New name (leave empty to keep current): ");
            string? newName = Console.ReadLine();
            newName = string.IsNullOrWhiteSpace(newName) ? null : newName;

            Console.Write("New price (leave empty to keep current): ");
            string? newPrice = Console.ReadLine();
            newPrice = string.IsNullOrWhiteSpace(newPrice) ? null : newPrice;

            Console.Write("New stock (leave empty to keep current): ");
            string? newStock = Console.ReadLine();
            newStock = string.IsNullOrWhiteSpace(newStock) ? null : newStock;

            Console.Write("New minimum age (leave empty for all ages / to keep current): ");
            string? newMinimumAge = Console.ReadLine();
            newMinimumAge = string.IsNullOrWhiteSpace(newMinimumAge) ? null : newMinimumAge;

            ShopItemService shopItemService = new();
            shopItemService.UpdateShopItem(selectedItem, newName, newPrice, newStock, newMinimumAge);

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}