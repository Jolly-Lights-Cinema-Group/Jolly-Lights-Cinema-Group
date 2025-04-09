class DiscountCode
{
    public string? Code { get; set; }
    public double DiscountAmount { get; set; }
    public string? DiscountType { get; set; }
    public DateTime ExperationDate { get; set; }
    public int OrderId { get; set; }

    public DiscountCode(string code, double discountamount, string discounttype, DateTime experationdate, int orderid)
    {
        Code = code;
        DiscountAmount = discountamount;
        DiscountType = discounttype;
        ExperationDate = experationdate;
        OrderId = orderid;
    }


}