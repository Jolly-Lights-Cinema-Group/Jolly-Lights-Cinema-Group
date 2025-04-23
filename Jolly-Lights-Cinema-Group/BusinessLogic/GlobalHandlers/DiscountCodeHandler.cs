using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Models;
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
                    Console.WriteLine();
                    return true;
                case 3:
                    Console.WriteLine();
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
            Console.WriteLine($"Discount Code name:");
            string? Type = Console.ReadLine();

            Console.WriteLine($"Discount Code: (Example: SPRING20,20TH-ANNIVERSARY,1-MILLION-COSTUMERS)");
            string Code = Console.ReadLine();

            Console.WriteLine($"What will the discount amount be?: (Example: 20% = 0.2, 30% = 0.3)");
            double DiscountAmount;
            while (!double.TryParse(Console.ReadLine(), out DiscountAmount))
            {
                Console.WriteLine("Invalid input. Please enter a number like 0.2 or 0.3:");
            }

            Console.WriteLine($"What will the experation month be: (YY/MM/DD)");
            DateTime ExperationDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine($"OrderId: (Leave Empty if none)");
            string? orderInput = Console.ReadLine();
            int? OrderId = string.IsNullOrWhiteSpace(orderInput) ? null : int.Parse(orderInput);

            Console.WriteLine($"Discount code = {Type},{Code},{DiscountAmount},{ExperationDate},{OrderId}");

            DiscountCode discountcode = new DiscountCode(Code, DiscountAmount, Type, ExperationDate, OrderId);

            DiscountCodeService discountcodeservice = new();
            discountcodeservice.RegisterDiscountCode(discountcode);

            Console.ReadKey();
        }

        private static void ChangeEmail()
        {

            Console.Clear();
            bool IsValid = false;
            do

            {
                Console.WriteLine($"To what do you want to change your Email {Globals.CurrentUser?.UserName}?");
                string email = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(email))
                {
                    EmployeeService employeeService = new EmployeeService();
                    employeeService.ChangeEmail(email, Globals.CurrentUser.UserName);
                    IsValid = true;
                }
            }
            while (!IsValid);
        }

        private static void ChangePassword()
        {
            Console.Clear();
            bool IsValid = false;

            do
            {
                Console.WriteLine($"To what do you want to change your password {Globals.CurrentUser?.UserName}?");
                string password = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(password))
                {
                    EmployeeService employeeService = new EmployeeService();
                    employeeService.ChangePassword(password, Globals.CurrentUser.UserName);
                    IsValid = true;
                }
            }
            while (!IsValid);
        }
    }

}