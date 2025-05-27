using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Helpers;

public static class ManageDiscountCodeMenu
{
    private static DiscountCodeService _discountCodeService = new();
    private static Menu _manageDiscountCodeMenu = new("Discount Code Menu.", new string[] { "Make Compensation Discount code", "Make General Discount code", "Delete Discount code", "Get Discountcode", "Back" });
    public static void ShowManageDiscountCodeMenu()
    {
        bool inManageDiscountCodeMenu = true;
        Console.Clear();

        while (inManageDiscountCodeMenu)
        {
            int userChoice = _manageDiscountCodeMenu.Run();
            inManageDiscountCodeMenu = HandleManageDiscountCodeChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleManageDiscountCodeChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                MakeCompensationDiscountCode();
                return true;
            case 1:
                MakeGeneralDiscountCode();
                return true;
            case 2:
                DeleteDiscountCode();
                return true;
            case 3:
                GetDiscountCode();
                return true;
            case 4:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    private static void MakeCompensationDiscountCode()
    {
        Console.Clear();
        DiscountCode? discountCode = _discountCodeService.MakeCompensationDiscountCode();
        if (discountCode != null)
        {
            Console.WriteLine($"Discount code has been added:\nCode: {discountCode.Code}\nDiscount Amount: {discountCode.DiscountAmount * 100}%\nDiscount Type/name: {discountCode.DiscountType}\nValid till: {discountCode.ExperationDate}");
        }
        else Console.WriteLine($"Discount code has not been added. Something went wrong.");

        Console.WriteLine("\nPress any key to continue");

        Console.ReadKey();
    }

    private static void MakeGeneralDiscountCode()
    {
        Console.Clear();

        string? Type;
        do
        {
            Console.Write($"Discount Code name: ");
            Type = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(Type));

        Console.WriteLine($"Discount Code: (Example: SPRING20,20TH-ANNIVERSARY,1-MILLION-COSTUMERS)");
        string? Code;
        do
        {
            Console.Write("Enter Discount Code: ");
            Code = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(Code) && _discountCodeService.CheckIfCodeExist(Code))
            {
                Console.WriteLine("This discount code already exists. Please enter a different one.");
            }
        } while (string.IsNullOrWhiteSpace(Code) || _discountCodeService.CheckIfCodeExist(Code));

        Console.WriteLine($"What will the discount amount be? (Example: 20% = 0.2, 30% = 0.3): ");
        double DiscountAmount;
        while (!double.TryParse(Console.ReadLine(), out DiscountAmount))
        {
            Console.WriteLine("Invalid input. Please enter a number like 0.2 or 0.3:");
        }

        DateTime expirationDate;
        do
        {
            Console.WriteLine("What is the expiration date? (dd/MM/yyyy): ");
            string? inputExpirationDate = Console.ReadLine();
            if (DateTimeValidator.TryParseDate(inputExpirationDate, out expirationDate))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
            }
        } while (true);


        Console.WriteLine($"OrderId (Leave Empty if none): ");
        string? orderInput = Console.ReadLine();
        int? OrderId = string.IsNullOrWhiteSpace(orderInput) ? null : int.Parse(orderInput);

        DiscountCode discountcode = new DiscountCode(Code, DiscountAmount, Type, expirationDate, OrderId);

        if (_discountCodeService.RegisterDiscountCode(discountcode))
        {
            Console.WriteLine($"Discount code has been added:\nCode: {discountcode.Code}\nDiscount Amount: {discountcode.DiscountAmount * 100}%\nDiscount Type/name: {discountcode.DiscountType}\nValid till: {discountcode.ExperationDate}");
        }
        else Console.WriteLine($"Discount code has not been added. Something went wrong.");

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

    private static void DeleteDiscountCode()
    {
        Console.Clear();
        string? code;
        do
        {
            Console.Write("What is the code of the Discount to delete?: ");
            code = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(code));

        if (!_discountCodeService.CheckIfCodeExist(code))
        {
            Console.Write("Discount code does not exist.");
        }
        else
        {
            if (_discountCodeService.DeleteDiscountCode(code))
            {
                Console.Clear();
                Console.WriteLine($"Discount has been deleted.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Discount code has not been deleted. Something went wrong.");
            }
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

    private static void GetDiscountCode()
    {
        Console.Clear();
        string? code;
        do
        {
            Console.Write("What is the code of the Discount: ");
            code = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(code));

        Console.Clear();

        if (!_discountCodeService.CheckIfCodeExist(code))
        {
            Console.Write("Discount code does not exist.");
        }

        else
        {
            DiscountCode? discountcode = _discountCodeService.GetDiscountCodeFromDB(code);
            if (discountcode != null)
            {
                Console.WriteLine($"Code: {discountcode.Code}\nDiscount Amount: {discountcode.DiscountAmount * 100}%\nDiscount Type/name: {discountcode.DiscountType}\nValid till: {discountcode.ExperationDate}");
            }
            else Console.WriteLine("Discountcode could not be found");
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }
}