using Jolly_Lights_Cinema_Group;

public class ShopMenu
{
    private readonly ShopItemService _shopitemService;

    public ShopMenu()
    {
        _shopitemService = new ShopItemService();
    }

    public void DisplayShop(Reservation reservation, int locationId)
    {
        bool inShop = true;
        int selectedIndex = 0;
        List<DateTime>? birthDates = null;

        while (inShop)
        {
            ShopItem? selectedItem = ShopItemMenu(ref selectedIndex, locationId);
            if (selectedItem == null)
            {
                inShop = false;
                continue;
            }

            if (selectedItem.MinimumAge > 0)
            {
                if (birthDates == null)
                    birthDates = AgeVerifier.AskDateOfBirth(selectedItem.MinimumAge);

                if (!AgeVerifier.IsOldEnough(birthDates, selectedItem.MinimumAge))
                {
                    Console.WriteLine($"You must be at least {selectedItem.MinimumAge} years old to buy {selectedItem.Name}.");
                    Console.WriteLine("\nPress any key to continue.");
                    Console.ReadKey();
                    continue;
                }
            }

            if (_shopitemService.SellShopItem(selectedItem, reservation))
            {
                Console.WriteLine($"{selectedItem.Name} added to reservation: {reservation.ReservationNumber}.");
            }

            else
            {
                Console.WriteLine($"{selectedItem.Name} could not be added to the reservation.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }

    public void CashDeskShop(int locationId)
    {
        Console.Clear();

        bool inShop = true;
        int selectedIndex = 0;
        List<DateTime>? birthDates = null;
        List<ShopItem> boughtItems = new List<ShopItem>();

        while (inShop)
        {
            ShopItem? selectedItem = ShopItemMenu(ref selectedIndex, locationId);
            if (selectedItem == null)
            {
                inShop = false;
                continue;
            }

            if (selectedItem.MinimumAge > 0)
            {
                if (birthDates == null)
                    birthDates = AgeVerifier.AskDateOfBirth(selectedItem.MinimumAge);

                if (!AgeVerifier.IsOldEnough(birthDates, selectedItem.MinimumAge))
                {
                    Console.WriteLine($"You must be at least {selectedItem.MinimumAge} years old to buy {selectedItem.Name}.");
                    Console.WriteLine("\nPress any key to continue.");
                    Console.ReadKey();
                    continue;
                }
            }

            if (_shopitemService.SellShopItem(selectedItem))
            {
                boughtItems.Add(selectedItem);
                Console.WriteLine($"{selectedItem.Name} added to basket.");
            }

            else
            {
                Console.WriteLine($"{selectedItem.Name} could not be added to basket.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        if (boughtItems.Count <= 0) return;

        Console.Clear();

        OrderLineService orderLineService = new();
        CustomerOrderService customerOrderService = new();

        List<OrderLine> orderLines = orderLineService.CreateOrderLineForCashDeskShopItems(boughtItems);
        CustomerOrder customerOrder = customerOrderService.CreateCustomerOrderForCashDesk(orderLines);

        Receipt.DisplayReceipt(orderLines, customerOrder);

        string? input;
        do
        {
            Console.Write("\nConfirm payment (y) or (c) to cancel: ");
            input = Console.ReadLine()?.Trim().ToLower();

            if (input == "y")
            {
                CustomerOrder? customerOrderId = customerOrderService.RegisterCustomerOrder(customerOrder);

                if (customerOrderId != null)
                {
                    for (int i = 0; i < orderLines.Count; i++)
                    {
                        orderLines[i].CustomerOrderId = customerOrderId.Id;
                    }

                    orderLineService.ConnectCustomerOrderIdToOrderLine(orderLines);
                }

                Console.WriteLine("\nPayment confirmed.");
                break;
            }

            if (input == "c")
            {
                Dictionary<int, int> quantities = new Dictionary<int, int>();

                foreach (ShopItem shopItem in boughtItems)
                {
                    if (quantities.ContainsKey(shopItem.Id!.Value))
                        quantities[shopItem.Id.Value]++;
                    else
                        quantities[shopItem.Id.Value] = 1;
                }

                foreach (KeyValuePair<int, int> kvp in quantities)
                {
                    ShopItem? item = _shopitemService.GetShopItemById(kvp.Key);
                    _shopitemService.RestoreShopItem(item!, kvp.Value);
                }

                Console.WriteLine("\nPayment cancelled");
                break;
            }

            else
            {
                Console.WriteLine("Invalid input. Please enter 'y' to confirm.");
            }
        } while (input != "y" && input != "c");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    public ShopItem? ShopItemMenu(ref int selectedIndex, int locationId)
    {
        List<ShopItem> availableItems = _shopitemService
            .GetAllShopItems(locationId)
            .Where(item => item.Stock > 0)
            .ToList();

        string[] menuItems = availableItems
            .Select(item => $"{item.Name}: €{Math.Round(item.Price * (1.0 + ((double)item.VatPercentage / 100)), 2)}")
            .Append("Finish")
            .ToArray();

        Menu shopMenu = new("Shop", menuItems, selectedIndex);
        int choice = shopMenu.Run();

        selectedIndex = choice;

        if (choice == availableItems.Count)
            return null;

        if (choice >= 0 && choice < availableItems.Count)
            return availableItems[choice];

        Console.WriteLine("Invalid choice.");
        Console.ReadKey();
        return null;
    }
}