using Jolly_Lights_Cinema_Group;
using JollyLightsCinemaGroup.BusinessLogic;

public class MakeReservationMenu
{
    private readonly ReservationService _reservationService;

    public MakeReservationMenu()
    {
        _reservationService = new ReservationService();
    }

    public Reservation? MakeReservation()
    {
        string? firstName;
        do
        {
            Console.Write("Enter first name: ");
            firstName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(firstName));

        string? lastName;
        do
        {
            Console.Write("Enter last name: ");
            lastName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(lastName));

        int phoneNumber;
        string? input;
        do
        {
            Console.Write("Enter telephone number: ");
            input = Console.ReadLine();
        } while (!int.TryParse(input, out phoneNumber) || phoneNumber < 0);

        string? eMail;
        do
        {
            Console.Write("Enter email address: ");
            eMail = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(eMail));

        string reservationNumber = ReservationNumberGenerator.GetReservationNumber();

        Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);
        if (_reservationService.RegisterReservation(reservation))
        {
            Console.WriteLine("Reservation added successfully.");
            return reservation;
        }
        Console.WriteLine("Reservation was not added to the database.");
        return null;
    }

    public void CompleteReservation(Reservation reservation, Movie selectedMovie, Schedule selectedSchedule, string selectedSeat)
    {
        Console.Write("\nWould you like to add extra items to your reservation? (y/n): ");
        string? response = Console.ReadLine()?.Trim().ToLower();

        if (response == "y")
        {
            ShopMenu shopMenu = new();
            shopMenu.DisplayShop(reservation);
        }

        OrderLineService orderLineService = new();
        orderLineService.CreateOrderLineForReservation(reservation);

        Console.Clear();

        Console.WriteLine($"Reservation Number: {reservation.ReservationNumber}");
        Console.WriteLine($"Movie: {selectedMovie.Title}");
        Console.WriteLine($"Date: {selectedSchedule.StartDate:dddd dd MMMM yyyy} {selectedSchedule.StartTime:hh\\:mm}");
        Console.WriteLine($"Seat(s): {selectedSeat}");

        Console.WriteLine("\nReservation complete. Press any key to continue.");
        Console.ReadKey();
    }
}
