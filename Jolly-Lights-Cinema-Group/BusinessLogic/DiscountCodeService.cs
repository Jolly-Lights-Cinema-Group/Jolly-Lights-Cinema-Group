using JollyLightsCinemaGroup.DataAccess;
using System.Text;

public class DiscountCodeService
{
    private static readonly char[] AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789".ToCharArray();

    private static readonly Random random = new();
    private readonly DiscountCodeRepository _discountCodeRepo;

    public DiscountCodeService()
    {
        _discountCodeRepo = new DiscountCodeRepository();
    }


    // function to make an discount code that is valid for 1 year. 
    // This function is meant as compensation for unhappy or angry customers.
    public void RegisterDiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
    {
        DiscountCode discountcode = new DiscountCode(code, discountAmount, discountType, experationDate, orderId);
        _discountCodeRepo.AddDiscountCode(discountcode);
    }

    public void RegisterDiscountCode(DiscountCode discountCode)
    {
        _discountCodeRepo.AddDiscountCode(discountCode);
    }
}