using System.Text;


namespace Jolly_Lights_Cinema_Group
{
    public static class DiscountCodeHandler
    {
        private static readonly char[] AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789".ToCharArray();
        private static readonly Random random = new();
        public static void ManageDiscountCode()
        {
            bool inDiscountCodeMenu = true;
            DiscountMenu ManageDiscountMenu = new();
            Console.Clear();

            while (inDiscountCodeMenu)
            {
                int userChoice = ManageDiscountMenu.Run();
                inDiscountCodeMenu = HandleManageAccountChoice(userChoice);
                Console.Clear();
            }
        }
        private static bool HandleManageAccountChoice(int choice)
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

        // Makes a 1 year compensation voucher. This way an employee or manager can compensate clients.
        private static void MakeCompensationDiscountCode()
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
            discountcodeservice.RegisterDiscountCode(discountcode);

            Console.ReadKey();
        }

        private static void MakeGeneralDiscountCode()
        {
            Console.Clear();
            string? Type;
            do
            {
                Console.Write($"Discount Code name:");
                Type = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(Type));

            Console.WriteLine($"Discount Code: (Example: SPRING20,20TH-ANNIVERSARY,1-MILLION-COSTUMERS)");
            string? Code;
            do
            {
                Console.Write($"Discount Code: (Example: SPRING20,20TH-ANNIVERSARY,1-MILLION-COSTUMERS)");
                Code = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(Code));

            Console.WriteLine($"What will the discount amount be?: (Example: 20% = 0.2, 30% = 0.3)");
            double DiscountAmount;
            while (!double.TryParse(Console.ReadLine(), out DiscountAmount))
            {
                Console.WriteLine("Invalid input. Please enter a number like 0.2 or 0.3:");
            }

            Console.WriteLine($"What will the experation month be: (DD/MM/YYYY)");
            DateTime ExperationDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine($"OrderId: (Leave Empty if none)");
            string? orderInput = Console.ReadLine();
            int? OrderId = string.IsNullOrWhiteSpace(orderInput) ? null : int.Parse(orderInput);

            DiscountCode discountcode = new DiscountCode(Code, DiscountAmount, Type, ExperationDate, OrderId);
            DiscountCodeService discountcodeservice = new();

            discountcodeservice.RegisterDiscountCode(discountcode);


            Console.ReadKey();
        }

        private static void DeleteDiscountCode()
        {

            Console.Clear();
            string? code;
            do
            {
                Console.Write("What is the code of the Discount to delete?:");
                code = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(code));

            DiscountCodeService discountcode = new();

            discountcode.DeleteDiscountCode(code);
            Console.ReadLine();
        }

        private static void GetDiscountCode()
        {
            Console.Clear();
            string? code;
            do
            {
                Console.Write("What is the code of the Discount:");
                code = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(code));

            DiscountCodeService discountcode = new();

            discountcode.GetDiscountCodeFromDB(code);
            Console.ReadLine();
        }
    }
}