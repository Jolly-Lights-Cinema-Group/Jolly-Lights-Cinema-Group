using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;

public static class AccountSettingsMenu
{
    private static EmployeeService _employeeService = new();
    private static Menu _accountSettingsMenu = new("Account Settings Menu.", new string[] { "Change Firstname", "Change Lastname", "Change Email", "Change Password" ,"Back" });
    public static void ShowAccountSettingsMenu()
    {
        bool inAccountSettingsMenu = true;
        Console.Clear();

        while (inAccountSettingsMenu)
        {
            int userChoice = _accountSettingsMenu.Run();
            inAccountSettingsMenu = HandleAccountSettingsChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleAccountSettingsChoice(int choice)
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
            string? firstName;
            do
            {
                Console.WriteLine($"To what do you want to change your firstname {Globals.CurrentUser?.UserName}?");
                firstName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(firstName));

            if (!string.IsNullOrWhiteSpace(firstName) && Globals.CurrentUser != null && Globals.CurrentUser.UserName != null)
            {
                if (_employeeService.ChangeFirstName(firstName, Globals.CurrentUser.UserName))
                {
                    Console.WriteLine($"Firstname changed to {firstName}.");
                }
                else
                {
                    Console.WriteLine("Firstname could not be changed.");
                }
                IsValid = true;
            }
        } while (!IsValid);

        Console.ReadKey();
    }

    public static void ChangeLastName()
    {
        Console.Clear();
        bool IsValid = false;
        do
        {
            string? lastName;
            do
            {
                Console.WriteLine($"To what do you want to change your lastname {Globals.CurrentUser?.UserName}?");
                lastName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(lastName));

            if (!string.IsNullOrWhiteSpace(lastName) && Globals.CurrentUser != null && Globals.CurrentUser.UserName != null)
            {
                if (_employeeService.ChangeLastName(lastName, Globals.CurrentUser.UserName))
                {
                    Console.WriteLine($"Lastname changed to {lastName}.");
                }
                else
                {
                    Console.WriteLine("Lastname could not be changed.");
                }
                IsValid = true;
            }
        } while (!IsValid);

        Console.ReadKey();
    }

    public static void ChangeEmail()
    {
        Console.Clear();
        bool IsValid = false;
        do
        {
            string? eMail;
            do
            {
                Console.WriteLine($"To what do you want to change your Email {Globals.CurrentUser?.UserName}?");
                eMail = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(eMail) || !eMail.Contains("@"));

            if (!string.IsNullOrWhiteSpace(eMail) && Globals.CurrentUser != null && Globals.CurrentUser.UserName != null)
            {
                if (_employeeService.ChangeEmail(eMail, Globals.CurrentUser.UserName))
                {
                    Console.WriteLine($"Email changed to {eMail}.");
                }
                else
                {
                    Console.WriteLine("Email could not be changed.");
                }
                IsValid = true;
            }
        }
        while (!IsValid);

        Console.ReadKey();
    }

    public static void ChangePassword()
    {
        Console.Clear();
        bool IsValid = false;
        do
        {
            string? password;
            do
            {
                Console.Write($"To what do you want to change your password {Globals.CurrentUser?.UserName}?");
                password = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(password));

            if (!string.IsNullOrWhiteSpace(password) && Globals.CurrentUser != null && Globals.CurrentUser.UserName != null)
            {
                if (_employeeService.ChangePassword(password, Globals.CurrentUser.UserName))
                {
                    Console.WriteLine($"Password changed to {password}.");
                }
                else
                {
                    Console.WriteLine("Password could not be changed.");
                }
                IsValid = true;
            }
        }
        while (!IsValid);

        Console.ReadKey();  
    }
}