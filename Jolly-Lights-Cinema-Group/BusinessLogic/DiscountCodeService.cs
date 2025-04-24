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

    public void RegisterDiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
    {
        DiscountCode discountcode = new DiscountCode(code, discountAmount, discountType, experationDate, orderId);
        if (_discountCodeRepo.AddDiscountCode(discountcode))
        {
            Console.Clear();
            Console.WriteLine($"Discount code has been added:\nCode: {code}\nDiscount Amount: {discountAmount * 100}%\nDiscount Type/name: {discountType}\nValid till: {experationDate}");
        }
        else
        {
            Console.Clear();
            { Console.WriteLine($"Discount code has not been added. Something went wrong."); }
        }
    }

    public void RegisterDiscountCode(DiscountCode discountCode)
    {
        if (_discountCodeRepo.AddDiscountCode(discountCode))
        {
            Console.Clear();
            Console.WriteLine($"Discount code has been added:\nCode: {discountCode.Code}\nDiscount Amount: {discountCode.DiscountAmount * 100}%\nDiscount Type/name: {discountCode.DiscountType}\nValid till: {discountCode.ExperationDate}");
        }
        else
        {
            Console.Clear();
            { Console.WriteLine($"Discount code has not been added. Something went wrong."); }
        }
    }

    public void GetDiscountCodeFromDB(string code)
    {
        Console.Clear();

        if (_discountCodeRepo.GetDiscountCode(code) != null)
        {
            DiscountCode Discount = _discountCodeRepo.GetDiscountCode(code);
            Console.WriteLine($"Discount exists:\nCode: {Discount.Code}\nAmount:{Discount.DiscountAmount * 100}%\nType: {Discount.DiscountType}\nValid till:{Discount.ExperationDate}");
        }
        else
        {
            Console.WriteLine($"Discount code {code} not found");
        }
    }

    public void DeleteDiscountCode(string code)
    {
        if (_discountCodeRepo.DeleteDiscountCode(code))
        {
            Console.Clear();
            Console.WriteLine($"Discount has been deleted.");
        }
        else
        {
            Console.Clear();
            { Console.WriteLine($"Discount code has not been deleted. Something went wrong."); }
        }
    }
}