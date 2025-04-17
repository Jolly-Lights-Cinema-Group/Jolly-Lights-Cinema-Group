class OrderLine
{
    public int? Id { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public int VatPercentage { get; set; }
    public double Price { get; set; }

    public OrderLine(int orderId, int quantity, string description, int vatPercentage, double price)
    {
        OrderId = orderId;
        Quantity = quantity;
        Description = description;
        VatPercentage = vatPercentage;
        Price = price;
    }

    public OrderLine(int id, int orderId, int quantity, string description, int vatPercentage, double price)
        : this(orderId, quantity, description, vatPercentage, price)
    {
        Id = id;
    }
}