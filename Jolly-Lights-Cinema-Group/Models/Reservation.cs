using System.Text;

public class Reservation
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhoneNumber { get; set; }
    public string EMail { get; set; }
    public string ReservationNumber { get; set; }
    public bool Paid { get; private set; } = false;

    public Reservation(string firstName, string lastName, int phoneNumber, string eMail)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EMail = eMail;
        ReservationNumber = CreateReservationNumber();
    }

    public Reservation(int id, string firstName, string lastName, int phoneNumber, string eMail, string reservationNumber, bool paid)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EMail = eMail;
        ReservationNumber = reservationNumber;
        Paid = paid;
    }

    public void Pay()
    {
        Paid = true;
    }

    private string CreateReservationNumber()
    {
        ReservationService reservationService = new();

        HashSet<string> existingReservationNumbers = reservationService.GetAllReservations().Select(r => r.ReservationNumber).ToHashSet();
        string reservationNumber;
        do
        {
            reservationNumber = GenerateReservationNumber(10);
        }
        while (existingReservationNumbers.Contains(reservationNumber));

        return reservationNumber;
    }

    private string GenerateReservationNumber(int length)
    {
        StringBuilder reservationNumber = new();

        char[] allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ1234567890".ToCharArray();
        Random random = new();

        for (int i = 0; i < length; i++)
        {
            char character = allowedChars[random.Next(allowedChars.Length)];
            reservationNumber.Append(character);
        }

        return reservationNumber.ToString();
    }
}
