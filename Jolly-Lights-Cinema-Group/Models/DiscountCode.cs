using System.Text;

public class DiscountCode
{
    public int? Id { get; set; }
    public string? Code { get; set; }
    public double DiscountAmount { get; set; }
    public string? DiscountType { get; set; }
    public DateTime ExperationDate { get; set; }
    public int? OrderId { get; set; }

    public DiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int? orderId)
    {
        Code = code;
        DiscountAmount = discountAmount;
        DiscountType = discountType;
        ExperationDate = experationDate;
        OrderId = orderId;
    }

    public DiscountCode(int id, string code, double discountAmount, string discountType, DateTime experationDate, int? orderId)
        : this(code, discountAmount, discountType, experationDate, orderId)
    {
        Id = id;
    }

    public static DiscountCode CreateWithCompensationDiscountCode()
    {
        StringBuilder code = new();
        Random random = new();
        
        int length = 10;
        char[] AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789".ToCharArray();

        for (int i = 0; i < length; i++)
        {
            char character = AllowedChars[random.Next(AllowedChars.Length)];
            code.Append(character);
        }

        string? Code = code.ToString();

        return new DiscountCode(Code, 0.2, "Compensation", DateTime.Now.AddYears(1), null);
    }
}