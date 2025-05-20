public class CustomerOrder
{
    public int? Id { get; set; }
    public double GrandPrice { get; set; }
    public DateTime PayDate { get; set; }
    public double Tax { get; set; }

    public CustomerOrder(double grandPrice, DateTime payDate, double tax)
    {
        GrandPrice = grandPrice;
        PayDate = payDate;
        Tax = tax;
    }

    public CustomerOrder(int id, double grandPrice, DateTime payDate, double tax)
        : this(grandPrice, payDate, tax)
    {
        Id = id;
    }
}
