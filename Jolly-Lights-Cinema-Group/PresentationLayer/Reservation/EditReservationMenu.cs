using Jolly_Lights_Cinema_Group;
using JollyLightsCinemaGroup.BusinessLogic;

public class EditReservationMenu
{
    private readonly ReservationService _reservationService;

    public EditReservationMenu()
    {
        _reservationService = new ReservationService();
    }

    public void EditReservation()
    {
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
            return;
        }

        string[] editReservationOptions = { "Edit shop items", "Cancel reservation", "Cancel" };

        Menu editReservationMenu = new($"Edit reservation: {reservation.ReservationNumber}", editReservationOptions);

        var inEditReservationsMenu = true;
        Console.Clear();

        while (inEditReservationsMenu)
        {
            int choice = editReservationMenu.Run();
            inEditReservationsMenu = HandleEditReservationMenu(choice, reservation);
            Console.Clear();
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