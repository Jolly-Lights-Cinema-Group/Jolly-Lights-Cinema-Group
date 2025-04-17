public class CustomerOrder
{
    public int? Id { get; set; }
    public double GrandPrice { get; set; }

    public CustomerOrder(double grandPrice)
    {
        GrandPrice = grandPrice;
    }

    public CustomerOrder(int id, double grandPrice)
        : this(grandPrice)
    {
        Id = id;
    }
}
