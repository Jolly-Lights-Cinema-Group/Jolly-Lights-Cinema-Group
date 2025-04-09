class Reservation
{
    public string? ReservationNumber { get; set; }
    public int? OrderId { get; set; }
    public int? Paid { get; set; }


    public Reservation(string reservationnumber, int orderid, int paid)
    {
        ReservationNumber = reservationnumber;
        OrderId = orderid;
        Paid = paid;
    }
}