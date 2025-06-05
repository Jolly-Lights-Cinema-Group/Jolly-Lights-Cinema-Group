using Jolly_Lights_Cinema_Group.Enum;

public class ScheduleSeat
{
    public int? Id { get; set; }
    public int ScheduleId { get; set; }
    public int? ReservationId { get; set; }
    public double Price { get; set; }
    public SeatType Type { get; set; }
    public string? SeatNumber { get; set; }

    public ScheduleSeat(int scheduleId, double price, SeatType type, string seatNumber, int? reservationId = null)
    {
        ScheduleId = scheduleId;
        ReservationId = reservationId;
        Price = price;
        Type = type;
        SeatNumber = seatNumber;
    }

    public ScheduleSeat(int id, int scheduleId, double price, SeatType type, string seatNumber, int? reservationId = null)
        : this(scheduleId, price, type, seatNumber, reservationId)
    {
        Id = id;
    }
}