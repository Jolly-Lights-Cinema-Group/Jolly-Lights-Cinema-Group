public class OrderLine
{
    public int? Id { get; set; }
    public int? ReservationId { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public int VatPercentage { get; set; }
    public double Price { get; set; }
    public int? CustomerOrderId { get; set; }

    public OrderLine(int quantity, string description, int vatPercentage, double price, int? reservationId = null, int? customerOrderId = null)
    {
        ReservationId = reservationId;
        Quantity = quantity;
        Description = description;
        VatPercentage = vatPercentage;
        Price = price;
        CustomerOrderId = customerOrderId;
    }

    public OrderLine(int id, int quantity, string description, int vatPercentage, double price, int? reservationId = null, int? customerOrderId = null)
        : this(quantity, description, vatPercentage, price, reservationId, customerOrderId)
    {
        Id = id;
    }
}