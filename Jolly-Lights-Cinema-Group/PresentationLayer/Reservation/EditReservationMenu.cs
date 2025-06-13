using Jolly_Lights_Cinema_Group;

public class EditReservationMenu
{
    private readonly ReservationService _reservationService;
    private readonly ScheduleShopItemService _scheduleShopItemService;
    private readonly ShopItemService _shopItemService;
    private readonly OrderLineService _orderLineService;

    public EditReservationMenu()
    {
        _reservationService = new ReservationService();
        _scheduleShopItemService = new ScheduleShopItemService();
        _shopItemService = new ShopItemService();
        _orderLineService = new OrderLineService();
    }

    public void EditReservation()
    {
        Console.Clear();

        Reservation? reservation = null;
        do
        {
            Console.Write("Enter reservation number to edit reservation (enter C tot cancel): ");
             string? reservationNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(reservationNumber))
            {
                Console.WriteLine("Reservation number cannot be empty.\n");
                continue;
            }

            if (reservationNumber.Trim().ToLower() == "c") return;

            reservation = _reservationService.FindReservationByReservationNumber(reservationNumber);

            if (reservation is null)
            {
                Console.WriteLine($"No Reservation found with reservation number: {reservationNumber}\n");
                continue;
            }

            break;

        } while (true);

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

    public bool HandleEditReservationMenu(int choice, Reservation reservation)
    {
        switch (choice)
        {
            case 0:
                EditShopItems(reservation);
                return true;
            case 1:
                DeleteReservation(reservation);
                return false;
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

        List<ScheduleShopItem> scheduleShopItems = _scheduleShopItemService.GetScheduleShopItemByReservation(reservation);
        int? locationId = _reservationService.GetLocationIdByReservation(reservation);

        if (locationId is null) return;

        if (scheduleShopItems.Count <= 0)
        {
            Console.WriteLine($"No shop items to edit");
            Console.WriteLine($"Do you want to add items to this reservation? (y/n): ");
            string? response = Console.ReadLine()?.Trim().ToLower();

            if (response == "y")
            {
                ShopMenu shopMenu = new();
                shopMenu.DisplayShop(reservation, (int)locationId);

                if (_orderLineService.DeleteOrderLineByReservation(reservation))
                {
                    _orderLineService.CreateOrderLineForReservation(reservation);
                }
            }

            return;
        }

        bool inCart = true;
        int selectedIndex = 0;

        while (inCart)
        {
            ShopItem? selectedItem = ReservedItems(scheduleShopItems, reservation, ref selectedIndex, (int)locationId);
            if (selectedItem == null)
            {
                inCart = false;
                continue;
            }

            if (_scheduleShopItemService.DeleteScheduleShopItem(selectedItem, reservation))
            {
                _shopItemService.RestoreShopItem(selectedItem);
                _orderLineService.DeleteOrderLineByReservation(reservation);
                _orderLineService.CreateOrderLineForReservation(reservation);

                Console.WriteLine($"{selectedItem.Name} removed from reservation: {reservation.ReservationNumber}.");
                scheduleShopItems = _scheduleShopItemService.GetScheduleShopItemByReservation(reservation);
            }

            else
            {
                Console.WriteLine($"{selectedItem.Name} could not be removed from reservation.");
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
            List<ScheduleShopItem> scheduleShopItems = _scheduleShopItemService.GetScheduleShopItemByReservation(reservation);

            if (_reservationService.DeleteReservation(reservation))
            {
                if (scheduleShopItems.Count > 0)
                {
                    foreach (ScheduleShopItem scheduleShopItem in scheduleShopItems)
                    {
                        ShopItem shopItem = _shopItemService.GetShopItemById(scheduleShopItem.ShopItemId)!;
                        _shopItemService.RestoreShopItem(shopItem);
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

    public ShopItem? ReservedItems(List<ScheduleShopItem> scheduleShopItems, Reservation reservation, ref int selectedIndex, int locationId)
    {
        List<ShopItem> shopItems = new();

        foreach (ScheduleShopItem scheduleShopItem in scheduleShopItems)
        {
            ShopItem? shopItem = _shopItemService.GetShopItemById(scheduleShopItem.ShopItemId);
            if (shopItem != null)
            {
                shopItems.Add(shopItem);
            }
        }

        string[] menuItems = shopItems
            .Select(item => $"{item.Name}: €{Math.Round(item.Price * (1.0 + ((double)item.VatPercentage / 100)), 2)}")
            .Append("Add items")
            .Append("Finish")
            .ToArray();

        Menu itemsInCart = new("Select item to delete", menuItems);
        int choice = itemsInCart.Run();

        selectedIndex = choice;

        if (choice == shopItems.Count + 1)
            return null;

        if (choice == shopItems.Count)
        {
            ShopMenu shopMenu = new();
            shopMenu.DisplayShop(reservation, locationId);

            _orderLineService.DeleteOrderLineByReservation(reservation);
            _orderLineService.CreateOrderLineForReservation(reservation);

            scheduleShopItems = _scheduleShopItemService.GetScheduleShopItemByReservation(reservation);
            return ReservedItems(scheduleShopItems, reservation, ref selectedIndex, locationId);
        }

        if (choice >= 0 && choice < shopItems.Count)
        {
            ShopItem selectedItem = shopItems[choice];
            return selectedItem;
        }

        Console.WriteLine("Invalid choice.");
        Console.ReadKey();
        return null;
    }
}