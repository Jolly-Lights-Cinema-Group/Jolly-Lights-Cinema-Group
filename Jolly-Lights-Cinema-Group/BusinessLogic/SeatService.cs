using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Models;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group.BusinessLogic;

public class SeatService
{
    private static List<Seat> _seats = [];
    
    public static void ManageSeats()
    {
        var seatRepo = new SeatRepository();
        var locationId = Globals.SessionLocationId;
        
        _seats = seatRepo.GetSeatPrices(locationId);
        
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
        
        var seatRepo = new SeatRepository();
        seatRepo.ModifySeatPrices(seat, newPrice, locationId);

        _seats = seatRepo.GetSeatPrices(locationId);
        return true;
    }
}