using Jolly_Lights_Cinema_Group;

public class EditReservationMenu
{
    private readonly ReservationService _reservationService;

    public EditReservationMenu()
    {
        _reservationService = new ReservationService();
    }

    public void EditReservation()
    {
        Console.Clear();

        string? reservationNumber;
        do
        {
            Console.Write("Enter reservation number to edit reservation: ");
            reservationNumber = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(reservationNumber));

        Reservation? reservation = _reservationService.FindReservationByReservationNumber(reservationNumber);

        if (reservation is null)
        {
            Console.WriteLine($"No Reservation found with reservation number: {reservationNumber}");
        }

        else
        {
            Console.Clear();
            string[] editReservationOptions = { "Edit shop items", "Cancel reservation", "Finish" };

            Menu editReservationMenu = new($"Edit reservation: {reservation.ReservationNumber}", editReservationOptions);

            var inEditReservationsMenu = true;
            Console.Clear();

            while (inEditReservationsMenu)
            {
                int choice = editReservationMenu.Run();
                inEditReservationsMenu = HandleEditReservationMenu(choice, reservation);
            }
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    public bool HandleEditReservationMenu(int choice, Reservation reservation)
    {
        switch (choice)
        {
            case 0:
                EditShopItems(reservation);
                return true;
            case 1:
                DeleteReservation(reservation);
                return true;
            case 2:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return false;
        }
    }

    public void EditShopItems(Reservation reservation)
    {
        Console.Clear();

        ScheduleShopItemService scheduleShopItemService = new();
        List<ScheduleShopItem> scheduleShopItems = scheduleShopItemService.GetScheduleShopItemByReservation(reservation);
        if (scheduleShopItems.Count <= 0)
        {
            Console.WriteLine($"No shop items to edit");
            Console.WriteLine($"Do you want to add items to this reservation? (y/n): ");
            string? response = Console.ReadLine()?.Trim().ToLower();

            if (response == "y")
            {
                ShopMenu shopMenu = new();
                shopMenu.DisplayShop(reservation);

                OrderLineService orderLineService = new();

                if (orderLineService.DeleteOrderLineByReservation(reservation))
                {
                    orderLineService.CreateOrderLineForReservation(reservation);
                }

                return;
            }
            else
            {
                return;
            }
        }

        List<ShopItem> shopItems = new();

        ShopItemService shopItemService = new();
        foreach (ScheduleShopItem scheduleShopItem in scheduleShopItems)
        {
            ShopItem? shopItem = shopItemService.GetShopItemById(scheduleShopItem.ShopItemId);
            if (shopItem != null)
            {
                shopItems.Add(shopItem);
            }
        }

        string[] menuItems = shopItems
            .Where(item => item.Stock > 0)
            .Select(item => $"{item.Name}: €{Math.Round(item.Price * (1.0 + ((double)item.VatPercentage / 100)), 2)}")
            .Append("Add items")
            .Append("Finish")
            .ToArray();

        Menu itemsInCart = new("Select item to delete", menuItems);

        bool inCart = true;

        while (inCart)
        {
            int choice = itemsInCart.Run();
            if (choice == shopItems.Count + 1)
            {
                inCart = false;
                continue;
            }

            if (choice == shopItems.Count)
            {
                ShopMenu shopMenu = new();
                shopMenu.DisplayShop(reservation);
                continue;
            }

            if (choice >= 0 && choice < shopItems.Count)
            {
                ShopItem selectedItem = shopItems[choice];

                if (scheduleShopItemService.DeleteScheduleShopItem(selectedItem, reservation))
                {
                    shopItemService.RestoreShopItem(selectedItem);
                    Console.WriteLine($"{selectedItem.Name} removed from reservation: {reservation.ReservationNumber}.");
                }
                else
                {
                    Console.WriteLine($"{selectedItem.Name} could not be removed from reservation.");
                }
            }

            else
            {
                Console.WriteLine("Invalid choice.");
            }

            OrderLineService orderLineService = new();

            if (orderLineService.DeleteOrderLineByReservation(reservation))
            {
                orderLineService.CreateOrderLineForReservation(reservation);
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }

    public void DeleteReservation(Reservation reservation)
    {
        Console.Clear();

        Console.WriteLine($"Delete reservation: {reservation.ReservationNumber}");
        Console.WriteLine($"Enter y to confirm: ");
        string? response = Console.ReadLine()?.Trim().ToLower();

        if (response == "y")
        {
            ScheduleShopItemService scheduleShopItemService = new();
            List<ScheduleShopItem> scheduleShopItems = scheduleShopItemService.GetScheduleShopItemByReservation(reservation);

            if (_reservationService.DeleteReservation(reservation))
            {
                if (scheduleShopItems.Count > 0)
                {
                    foreach (ScheduleShopItem scheduleShopItem in scheduleShopItems)
                    {
                        ShopItemService shopItemService = new();
                        ShopItem shopItem = shopItemService.GetShopItemById(scheduleShopItem.ShopItemId)!;
                        shopItemService.RestoreShopItem(shopItem);
                    }
                }
                Console.WriteLine($"Reservation: {reservation.ReservationNumber} deleted");
            }
            else
            {
                Console.WriteLine($"Reservation: {reservation.ReservationNumber} could not be deleted");
            }
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }
}