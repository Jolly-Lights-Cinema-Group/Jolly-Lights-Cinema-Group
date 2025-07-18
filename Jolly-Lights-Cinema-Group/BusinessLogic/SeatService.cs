using Jolly_Lights_Cinema_Group.Enum;
using Jolly_Lights_Cinema_Group.Models;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group.BusinessLogic;

public class SeatService
{
    private static SeatRepository _seatRepo = new();

    public List<Seat> GetSeatPrices(int locationId)
    {
        return _seatRepo.GetSeatPrices(locationId);
    }

    public bool EditSeatPrice(Seat seat, decimal newPrice, int locationId)
    {
        return _seatRepo.ModifySeatPrices(seat, newPrice, locationId);
    }

    public double GetSeatPriceForSeatTypeOnLocation(SeatType type, int locationId)
    {
        return _seatRepo.GetSeatPriceForSeatTypeOnLocation(type, locationId);
    }
}