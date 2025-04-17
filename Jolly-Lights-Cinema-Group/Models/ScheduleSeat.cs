class ScheduleSeat
{
    public int? Id { get; set; }
    public int ScheduleId { get; set; }
    public int ReservationId { get; set; }
    public double Price { get; set; }
    public int Type { get; set; }
    public string? SeatNumber { get; set; }

    public ScheduleSeat(int scheduleId, int reservationId, double price, int type, string seatNumber)
    {
        ScheduleId = scheduleId;
        ReservationId = reservationId;
        Price = price;
        Type = type;
        SeatNumber = seatNumber;
    }

    public ScheduleSeat(int id, int scheduleId, int reservationId, double price, int type, string seatNumber)
        : this(scheduleId, reservationId, price, type, seatNumber)
    {
        Id = id;
    }
}