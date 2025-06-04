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
        bool inShop = true;
        int selectedIndex = 0;

        while (inShop)
        {
            ShopItem? selectedItem = ShopItemMenu(ref selectedIndex);
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

    public void CashDeskShop()
    {
        // Console.Clear();

        // List<ShopItem> selectedShopItems = new List<ShopItem>();
        // List<ShopItem> shopItems = _shopitemService.GetAllShopItems();

        // bool inShop = true;
        // (Menu shopMenu, List<ShopItem> availableItems) = ShopItemMenu();

        // while (inShop)
        // {
        //     int choice = shopMenu.Run();
        //     if (choice == shopItems.Count)
        //     {
        //         inShop = false;
        //         continue;
        //     }
        // }
        
        // else
        // {
        //     List<ShopItem> selectedShopItems = new List<ShopItem>();
        //     List<ShopItem> shopItems = _shopitemService.GetAllShopItems();
        //     bool inShop = true;
        //     Menu shopMenu = ShopItemMenu();

            //     while (inShop)
            //     {
            //         int choice = shopMenu.Run();
            //         if (choice == shopItems.Count)
            //         {
            //             inShop = false;
            //             continue;
            //         }

            //         if (choice >= 0 && choice < shopItems.Count)
            //         {
            //             ShopItem selectedItem = shopItems[choice];
            //             if (selectedItem.Stock <= 0)
            //             {
            //                 Console.WriteLine($"{selectedItem.Name} out of stock.");
            //                 Console.WriteLine("\nPress any key to continue.");
            //                 Console.ReadKey();
            //                 continue;
            //             }

            //             if (_shopitemService.SellShopItem(selectedItem, reservation))
            //             {
            //                 selectedShopItems.Add(selectedItem);
            //                 Console.WriteLine($"{selectedItem.Name} added to reservation: {reservation.ReservationNumber}.");
            //             }
            //             else
            //             {
            //                 Console.WriteLine($"{selectedItem.Name} could not be added to the reservation.");
            //             }
            //         }

            //         else Console.WriteLine("Invalid choice.");

            //         Console.WriteLine("\nPress any key to continue.");
            //         Console.ReadKey();
            //     }

            //     Console.Clear();

            //     OrderLineService orderLineService = new();
            //     CustomerOrderService customerOrderService = new();

            //     List<OrderLine> orderLines = orderLineService.CreateOrderLineForScheduleShopItem(reservation, selectedShopItems);
            //     CustomerOrder customerOrder = customerOrderService.CreateCustomerOrderForCashDesk(orderLines);

            //     foreach (OrderLine orderLine in orderLines)
            //     {
            //         Console.WriteLine($"{orderLine.Description} * {orderLine.Quantity} = €{orderLine.Price}     ({orderLine.VatPercentage}% VAT)");
            //     }
            //     Console.WriteLine($"-----------------------------------------------------------------------");
            //     Console.WriteLine($"Subtotal (excl. Tax): €{Math.Round(customerOrder.GrandPrice - customerOrder.Tax, 2)}");
            //     Console.WriteLine($"VAT: €{customerOrder.Tax}");
            //     Console.WriteLine($"Total (incl. Tax): €{customerOrder.GrandPrice}");

            //     string? input;
            //     do
            //     {
            //         Console.Write("Confirm payment? (y): ");
            //         input = Console.ReadLine()?.Trim().ToLower();

            //         if (input == "y")
            //         {
            //             if (customerOrderService.RegisterCustomerOrder(customerOrder))
            //             {
            //                 Console.WriteLine("Payment confirmed.");
            //                 break;
            //             }
            //             Console.WriteLine("Payment could not be confirmed.");
            //             break;
            //         }

            //         else
            //         {
            //             Console.WriteLine("Invalid input. Please enter 'y' to confirm.");
            //         }

            //     } while (input != "y");

            //     Console.WriteLine("\nPress any key to continue.");
            //     Console.ReadKey();
            // }
    }

    public ShopItem? ShopItemMenu(ref int selectedIndex)
    {
        List<ShopItem> availableItems = _shopitemService
            .GetAllShopItems()
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