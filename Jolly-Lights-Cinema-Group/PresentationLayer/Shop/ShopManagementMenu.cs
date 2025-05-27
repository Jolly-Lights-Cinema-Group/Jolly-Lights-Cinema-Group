using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;

public static class ShopManagementMenu
{
    private static ShopItemService _shopItemService = new();
    private static Menu _manageMovieMenu = new("Shop Management", new string[] { "Add item", "Edit item", "Back" });
    public static void ShowShopManagementMenu()
    {
        bool inShopManagementMenu = true;
        Console.Clear();

        while (inShopManagementMenu)
        {
            int userChoice = _manageMovieMenu.Run();
            inShopManagementMenu = HandleShopManagementChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleShopManagementChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                AddShopItem();
                return true;
            case 1:
                EditShopItem();
                return true;
            case 2:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    private static void AddShopItem()
    {
        Console.Clear();
        Console.WriteLine("Information to add item to shop:");

        int locationId;
        if (Globals.CurrentUser!.Role == Role.Admin)
        {

            LocationMenu location = new();
            int selectedLocation = location.Run();

            LocationService locationService = new LocationService();
            List<Location> locations = locationService.GetAllLocations();

            locationId = (int)locations[selectedLocation].Id!;
        }
        else locationId = Globals.SessionLocationId;

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
            Console.Write("Enter the price of the item (€) (excl. VAT): ");
            inputPrice = Console.ReadLine();
        } while (!double.TryParse(inputPrice, out price) || price < 0);

        int stock;
        string? inputStock;
        do
        {
            Console.Write("Enter the stock: ");
            inputStock = Console.ReadLine();
        } while (!Int32.TryParse(inputStock, out stock) || stock < 0);


        string[] vatOptions = { "9%", "21%" };
        Menu vatmenu = new("Select VAT percentage:", vatOptions);
        int vatChoice = vatmenu.Run();

        int vat;
        if (vatChoice == 0)
        {
            vat = 9;
        }
        else
        {
            vat = 21;
        }

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
            shopItem = new(name, price, stock, locationId, vat, minimumAge);
        }
        else
        {
            shopItem = new(name, price, stock, locationId, vat);
        }

        if (_shopItemService.RegisterShopItem(shopItem))
        {
            Console.WriteLine("Item successfully added to the shop.");
            return;
        }

        else Console.WriteLine("Item could not be added to the shop.");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    public static void EditShopItem()
    {
        Console.Clear();

        int locationId;
        if (Globals.CurrentUser!.Role == Role.Admin)
        {

            LocationMenu location = new();
            int selectedLocation = location.Run();

            LocationService locationService = new LocationService();
            List<Location> locations = locationService.GetAllLocations();

            locationId = (int)locations[selectedLocation].Id!;
        }
        else locationId = Globals.SessionLocationId;

        Console.Clear();

        List<ShopItem> shopItems = _shopItemService.GetAllShopItems(locationId);

        string[] menuItems = shopItems
            .Select(item => $"Name: {item.Name}; Price: €{item.Price}; Stock: {item.Stock}; VAT: {item.VatPercentage}%; Minimum age: {item.MinimumAge}")
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

        if (_shopItemService.UpdateShopItem(selectedItem, newName, newPrice, newStock, newMinimumAge))
        {
            Console.WriteLine($"{selectedItem.Name} is updated");
        }
        else Console.WriteLine("No item found in shop to update.");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }
}
