public class CustomerOrder
{
    public int? Id { get; set; }
    public double GrandPrice { get; set; }
    public DateTime PayDate { get; set; }

    public CustomerOrder(double grandPrice, DateTime payDate)
    {
        GrandPrice = grandPrice;
        PayDate = payDate;
    }

    public CustomerOrder(int id, double grandPrice, DateTime payDate)
        : this(grandPrice, payDate)
    {
        Id = id;
    }
}
