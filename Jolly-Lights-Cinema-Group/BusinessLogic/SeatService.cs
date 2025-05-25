using Jolly_Lights_Cinema_Group.Common;
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
}