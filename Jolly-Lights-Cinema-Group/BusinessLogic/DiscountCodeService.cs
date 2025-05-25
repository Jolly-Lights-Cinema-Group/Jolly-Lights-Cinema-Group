using System.Text;
using JollyLightsCinemaGroup.DataAccess;

public class DiscountCodeService
{
    private static readonly char[] AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789".ToCharArray();

    private static readonly Random random = new();
    private readonly DiscountCodeRepository _discountCodeRepo;

    public DiscountCodeService()
    {
        _discountCodeRepo = new DiscountCodeRepository();
    }

    public bool RegisterDiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
    {
        DiscountCode discountcode = new DiscountCode(code, discountAmount, discountType, experationDate, orderId);
        if (!_discountCodeRepo.CheckIfCodeExist(discountcode.Code!))
        {
            return _discountCodeRepo.AddDiscountCode(discountcode);
        }
        return false;
    }

    public bool RegisterDiscountCode(DiscountCode discountCode)
    {
        if (!_discountCodeRepo.CheckIfCodeExist(discountCode.Code!))
        {
            return _discountCodeRepo.AddDiscountCode(discountCode);
        }
        return false;
    }

    public DiscountCode? MakeCompensationDiscountCode()
    {
        Console.Clear();
        StringBuilder code = new();
        int length = 10;

        for (int i = 0; i < length; i++)
        {
            char character = AllowedChars[random.Next(AllowedChars.Length)];
            code.Append(character);
        }

        string? Code = code.ToString();

        DiscountCode discountcode = new DiscountCode(Code, 0.2, "Compensation", DateTime.Now.AddYears(1), null);

        DiscountCodeService discountcodeservice = new();
        if (_discountCodeRepo.AddDiscountCode(discountcode))
        {
            return discountcode;
        }

        return null;
    }

    public DiscountCode? GetDiscountCodeFromDB(string code) // Getting a discount code back. Delete the messages.
    {
        return _discountCodeRepo.GetDiscountCode(code);
    }

    public bool DeleteDiscountCode(string code)
    {
        return _discountCodeRepo.DeleteDiscountCode(code);
    }

    public bool CheckIfCodeExist(string code)
    {
        return _discountCodeRepo.CheckIfCodeExist(code);
    }
}