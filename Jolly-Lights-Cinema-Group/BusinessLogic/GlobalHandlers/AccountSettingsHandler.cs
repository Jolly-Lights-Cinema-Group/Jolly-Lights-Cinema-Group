using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Models;

namespace Jolly_Lights_Cinema_Group
{
    public static class AccountSettingsHandler
    {
        public static void ManageAccount()
        {
            bool inManageAccountMenu = true;
            AccountSettingsMenu ManageAccountSettings = new();
            Console.Clear();

            while (inManageAccountMenu)
            {
                int userChoice = ManageAccountSettings.Run();
                inManageAccountMenu = HandleManageAccountChoice(userChoice);
                Console.Clear();
            }
        }
        private static bool HandleManageAccountChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    ChangeFirstName();
                    return true;
                case 1:
                    ChangeLastName();
                    return true;
                case 2:
                    ChangeEmail();
                    return true;
                case 3:
                    ChangePassword();
                    return true;
                case 4:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }

        private static void ChangeFirstName()
        {
            Console.Clear();
            bool IsValid = false;
            do
            {
                Console.WriteLine($"To what do you want to change your firstname {Globals.CurrentUser?.UserName}?");
                string? Firstname = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(Firstname))
                {
                    EmployeeService employeeService = new EmployeeService();
                    employeeService.ChangeFirstName(Firstname, Globals.CurrentUser.UserName);
                    IsValid = true;
                }
            }
            while (!IsValid);

            Console.ReadKey();
        }

        private static void ChangeLastName()
        {
            Console.Clear();
            bool IsValid = false;
            do
            {
                Console.WriteLine($"To what do you want to change your lastname {Globals.CurrentUser?.UserName}?");
                string? lastname = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(lastname))
                {
                    EmployeeService employeeService = new EmployeeService();
                    employeeService.ChangeLastName(lastname, Globals.CurrentUser.UserName);
                    IsValid = true;
                }
            }
            while (!IsValid);

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