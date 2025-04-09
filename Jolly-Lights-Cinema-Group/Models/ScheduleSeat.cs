class ScheduleSeat
{
    public int ScheduleId { get; set; }
    public int ReservationId { get; set; }
    public double Price { get; set; }
    public int Type { get; set; }
    public string? SeatNumber { get; set; }

    public ScheduleSeat(int scheduleid, int reservationid, double price, int type, string seatnumber)
    {
        ScheduleId = scheduleid;
        ReservationId = reservationid;
        Price = price;
        Type = type;
        SeatNumber = seatnumber;
    }

}