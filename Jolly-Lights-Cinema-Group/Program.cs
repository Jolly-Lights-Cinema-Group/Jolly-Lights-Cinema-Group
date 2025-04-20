using Jolly_Lights_Cinema_Group.BusinessLogic;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;
using Jolly_Lights_Cinema_Group.Models;
using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    class Program
    {
        static void Main()
        {
            DatabaseManager.InitializeDatabase();

            // Main menu / Asking for location
            LocationMenu location = new();
            int selectedLocation = location.Run();

            Console.Clear();

            string userName;
            string password;
            do
            {
                Console.WriteLine("Login.");
                Console.WriteLine("Username: ");
                userName = Console.ReadLine()!;
                Console.WriteLine("Password: ");
                password = Console.ReadLine()!;
            } while (!AuthenticationService.Login(userName: userName, password: password));

            Console.Clear();
            Console.WriteLine($"Login successfull!");

            var user = Globals.CurrentUser;

            while (user!.IsAuthenticated)
            {
                switch (user.Role)
                {
                    case Role.Admin:
                        AdminMenu adminMenu = new AdminMenu();
                        int adminChoice = adminMenu.Run();
                        AdminChoiceHandler.AdminMainMenu = true;
                        AdminChoiceHandler.HandleChoice(adminChoice, ref user);
                        break;

                    case Role.Manager:
                        ManagerMenu managerMenu = new ManagerMenu();
                        int managerChoice = managerMenu.Run();
                        ManagerChoiceHandler.HandleChoice(managerChoice, ref user);
                        break;

                    case Role.Employee:
                        EmployeeMenu employeeMenu = new EmployeeMenu();
                        int employeeChoice = employeeMenu.Run();
                        EmployeeChoiceHandler.HandleChoice(employeeChoice, ref user);
                        break;

                    default:
                        Console.WriteLine("Invalid role selected.");
                        break;
                }
            }
            Console.Clear();
            AuthenticationService.Logout();
            Console.WriteLine($"Succesfully logged out!");
        }
    }
}