public class Reservation
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhoneNumber { get; set; }
    public string EMail{ get; set; }
    public string ReservationNumber { get; set; }
    public int OrderId { get; set; }
    public bool Paid { get; set; }


    public Reservation(string firstName, string lastName, int phoneNumber, string eMail, string reservationnumber, int orderid, bool paid)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EMail = eMail;
        ReservationNumber = reservationnumber;
        OrderId = orderid;
        Paid = paid;
    }
}
