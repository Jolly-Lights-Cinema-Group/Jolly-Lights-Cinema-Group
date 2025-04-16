class DiscountCode
{
    public int? Id { get; set; }
    public string? Code { get; set; }
    public double DiscountAmount { get; set; }
    public string? DiscountType { get; set; }
    public DateTime ExperationDate { get; set; }
    public int OrderId { get; set; }

    public DiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
    {
        Code = code;
        DiscountAmount = discountAmount;
        DiscountType = discountType;
        ExperationDate = experationDate;
        OrderId = orderId;
    }

    public DiscountCode(int id, string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
        : this(code, discountAmount, discountType, experationDate, orderId)
    {
        Id = id;
    }
}