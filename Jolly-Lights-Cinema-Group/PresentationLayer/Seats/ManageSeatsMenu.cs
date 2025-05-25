using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.BusinessLogic;
using Jolly_Lights_Cinema_Group.Models;

public static class ManageSeatsMenu
{
    private static SeatService _seatService = new();
    private static List<Seat> _seats = [];

    public static void ManageSeats()
    {
        Console.Clear();

        LocationMenu location = new();
        int selectedLocation = location.Run();

        LocationService locationService = new LocationService();
        List<Location> locations = locationService.GetAllLocations();

        int locationId = (int)locations[selectedLocation].Id!;

        _seats = _seatService.GetSeatPrices(locationId);

        Menu seatMenu = new("Seats", _seats
            .Select(item => $"{item.Type}: €{item.Price}")
            .Append("Back")
            .ToArray());

        var managing = true;
        while (managing)
        {
            var choice = seatMenu.Run();
            managing = HandleChoice(choice, locationId);
            seatMenu = new("Seats", _seats
                .Select(item => $"{item.Type}: €{item.Price}")
                .Append("Back")
                .ToArray());
            Console.Clear();
        }
    }
    private static bool HandleChoice(int choice, int locationId)
    {
        if (choice == _seats.Count)
        {
            return false;
        }

        var seat = _seats[choice];
        Console.Clear();

        Console.WriteLine("Enter the new price");
        var newPrice = Convert.ToDecimal(Console.ReadLine());

        if (_seatService.EditSeatPrice(seat, newPrice, locationId))
        {
            _seats = _seatService.GetSeatPrices(locationId);
            Console.WriteLine("Seat price updated");
        }

        else Console.WriteLine("Seat price could not be updated");

        return true;
    }
}
