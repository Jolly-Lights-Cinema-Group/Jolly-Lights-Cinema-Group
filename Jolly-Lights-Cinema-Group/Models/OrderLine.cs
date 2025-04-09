class OrderLine
{
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public int VatPercentage { get; set; }
    public double Price { get; set; }

    public OrderLine(int orderid, int quantity, string description, int vatpercentage, double price)
    {
        OrderId = orderid;
        Quantity = quantity;
        Description = description;
        VatPercentage = vatpercentage;
        Price = price;
    }
}