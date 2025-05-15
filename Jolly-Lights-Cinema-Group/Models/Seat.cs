using Jolly_Lights_Cinema_Group.Enum;

namespace Jolly_Lights_Cinema_Group.Models;

public class Seat
{
    public int Id { get; set; }
    public SeatType Type { get; set; }
    public double Price { get; set; }
}