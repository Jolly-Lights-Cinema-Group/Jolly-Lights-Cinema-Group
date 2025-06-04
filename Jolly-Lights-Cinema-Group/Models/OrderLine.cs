public class OrderLine
{
    public int? Id { get; set; }
    public int? ReservationId { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public int VatPercentage { get; set; }
    public double Price { get; set; }

    public OrderLine(int quantity, string description, int vatPercentage, double price, int? reservationId = null)
    {
        ReservationId = reservationId;
        Quantity = quantity;
        Description = description;
        VatPercentage = vatPercentage;
        Price = price;
    }

    public OrderLine(int id, int quantity, string description, int vatPercentage, double price, int? reservationId = null)
        : this(quantity, description, vatPercentage, price, reservationId)
    {
        Id = id;
    }
}