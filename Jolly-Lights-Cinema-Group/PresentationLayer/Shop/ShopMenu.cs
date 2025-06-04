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

        while (inShop)
        {
            ShopItem? selectedItem = ShopItemMenu(ref selectedIndex, locationId);
            if (selectedItem == null)
            {
                inShop = false;
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

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }

    public void CashDeskShop(int locationId)
    {
        Console.Clear();

        bool inShop = true;
        int selectedIndex = 0;
        List<ShopItem> boughtItems = new List<ShopItem>();

        while (inShop)
        {
            ShopItem? selectedItem = ShopItemMenu(ref selectedIndex, locationId);
            if (selectedItem == null)
            {
                inShop = false;
                continue;
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

        Console.Clear();

        OrderLineService orderLineService = new();
        CustomerOrderService customerOrderService = new();

        List<OrderLine> orderLines = orderLineService.CreateOrderLineForCashDeskShopItems(boughtItems);
        CustomerOrder customerOrder = customerOrderService.CreateCustomerOrderForCashDesk(orderLines);

        foreach (OrderLine orderLine in orderLines)
        {
            Console.WriteLine($"{orderLine.Description} * {orderLine.Quantity} = €{orderLine.Price}     ({orderLine.VatPercentage}% VAT)");
        }
        Console.WriteLine($"-----------------------------------------------------------------------");
        Console.WriteLine($"Subtotal (excl. Tax): €{Math.Round(customerOrder.GrandPrice - customerOrder.Tax, 2)}");
        Console.WriteLine($"VAT: €{customerOrder.Tax}");
        Console.WriteLine($"Total (incl. Tax): €{customerOrder.GrandPrice}");

        string? input;
        do
        {
            Console.Write("\nConfirm payment? (y): ");
            input = Console.ReadLine()?.Trim().ToLower();

            if (input == "y")
            {
                customerOrderService.RegisterCustomerOrder(customerOrder);
                Console.WriteLine("\nPayment confirmed.");
                break;
            }

            else
            {
                Console.WriteLine("Invalid input. Please enter 'y' to confirm.");
            }
        } while (input != "y");

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