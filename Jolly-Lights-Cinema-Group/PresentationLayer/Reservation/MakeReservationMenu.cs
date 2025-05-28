using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;

public class MakeReservationMenu
{
    private readonly ReservationService _reservationService;
    private readonly ScheduleSeatService _scheduleSeatService;

    public MakeReservationMenu()
    {
        _reservationService = new ReservationService();
        _scheduleSeatService = new ScheduleSeatService();
    }

    public void MakeReservation()
    {
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

        MovieScheduleMenu movieScheduleMenu = new();
        Movie? selectedMovie = movieScheduleMenu.SelectMovieMenu(locationId);

        if (selectedMovie is null) return;

        MovieDateTimeMenu movieDateTimeMenu = new();
        Schedule? selectedSchedule = movieDateTimeMenu.SelectSchedule(selectedMovie, locationId);

        if (selectedSchedule is null) return;

        MovieRoomService movieRoomService = new();
        MovieRoom? movieRoom = movieRoomService.GetMovieRoomById(selectedSchedule.MovieRoomId);

        if (movieRoom is null) return;

        SeatSelection seatSelection = new();
        ScheduleSeat selectedSeat = seatSelection.SelectSeatsMenu(locationId, movieRoom, selectedSchedule);

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
        Reservation? newReservation;

        if (_reservationService.RegisterReservation(reservation))
        {
            newReservation = _reservationService.FindReservationByReservationNumber(reservation.ReservationNumber);
            if (newReservation != null)
            {
                ScheduleSeat scheduleSeat = new(selectedSeat.ScheduleId, newReservation.Id!.Value, selectedSeat.Price, selectedSeat.Type, selectedSeat.SeatNumber!);
                if (_scheduleSeatService.AddSeatToReservation(scheduleSeat))
                {
                    Console.WriteLine("Reservation added successfully.");
                }
                else return;
            }
            else return;
        }
        else
        {
            Console.WriteLine("Reservation was not added to the database.");
            return;
        }

        Console.Write("\nWould you like to add extra items to your reservation? (y/n): ");
        string? response = Console.ReadLine()?.Trim().ToLower();

        if (response == "y")
        {
            ShopMenu shopMenu = new();
            shopMenu.DisplayShop(newReservation);
        }

        OrderLineService orderLineService = new();
        orderLineService.CreateOrderLineForReservation(newReservation);

        Console.Clear();

        Console.WriteLine($"Reservation Number: {reservation.ReservationNumber}");
        Console.WriteLine($"Movie: {selectedMovie.Title}");
        Console.WriteLine($"Date: {selectedSchedule.StartDate:dddd dd MMMM yyyy} {selectedSchedule.StartTime:hh\\:mm}");
        Console.WriteLine($"Seat(s): {selectedSeat.SeatNumber}");

        Console.WriteLine("\nReservation complete. Press any key to continue.");
        Console.ReadKey();
    }
}
