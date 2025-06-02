using Jolly_Lights_Cinema_Group.Enum;

public class ScheduleSeat
{
    public int? Id { get; set; }
    public int ScheduleId { get; set; }
    public int? ReservationId { get; set; }
    public double Price { get; set; }
    public SeatType Type { get; set; }
    public string? SeatNumber { get; set; }

    public ScheduleSeat(int scheduleId, int reservationId, double price, SeatType type, string seatNumber)
    {
        ScheduleId = scheduleId;
        ReservationId = reservationId;
        Price = price;
        Type = type;
        SeatNumber = seatNumber;
    }

    public ScheduleSeat(int id, int scheduleId, int reservationId, double price, SeatType type, string seatNumber)
        : this(scheduleId, reservationId, price, type, seatNumber)
    {
        Id = id;
    }

    public ScheduleSeat(int scheduleId, double price, SeatType type, string seatNumber)
    {
        ScheduleId = scheduleId;
        Price = price;
        Type = type;
        SeatNumber = seatNumber;
    }
}