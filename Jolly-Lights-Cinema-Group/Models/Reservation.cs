public class Reservation
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhoneNumber { get; set; }
    public string EMail{ get; set; }
    public string ReservationNumber { get; set; }
    public int OrderId { get; set; }
    public bool Paid { get; set; }

    public Reservation(string firstName, string lastName, int phoneNumber, string eMail, string reservationNumber, int orderId, bool paid)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EMail = eMail;
        ReservationNumber = reservationNumber;
        OrderId = orderId;
        Paid = paid;
    }

    public Reservation(int id, string firstName, string lastName, int phoneNumber, string eMail, string reservationNumber, int orderId, bool paid)
        : this(firstName, lastName, phoneNumber, eMail, reservationNumber, orderId, paid)
    {
        Id = id;
    }
}
