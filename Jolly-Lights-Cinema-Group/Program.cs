﻿using Jolly_Lights_Cinema_Group.BusinessLogic;
using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    class Program
    {
        static void Main()
        {
            DatabaseManager.InitializeDatabase();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Main menu / Asking for location
            while (true)
            {
                LocationMenu location = new();
                int selectedLocation = location.Run();

                LocationService locationService = new LocationService();
                List<Location> locations = locationService.GetAllLocations();

                Globals.SessionLocationId = (int)locations[selectedLocation].Id!;

                Console.Clear();

                AuthenticationService authenticationService = new AuthenticationService();
                string userName;
                string password;
                do
                {
                    Console.WriteLine("Login");
                    Console.WriteLine("Enter Username: ");
                    userName = Console.ReadLine()!;
                    Console.WriteLine("Enter Password: ");
                    password = Console.ReadLine()!;
                } while (!authenticationService.Login(userName: userName, password: password));

                Console.Clear();
                Console.WriteLine($"Login successfull!");

                var user = Globals.CurrentUser;

                while (user!.IsAuthenticated)
                {
                    Console.Clear();
                    switch (user.Role)
                    {
                        case Role.Admin:
                            AdminMenu.ShowAdminMenu(ref user);
                            break;

                        case Role.Manager:
                            ManagerMenu.ShowManagerMenu(ref user);
                            break;

                        case Role.Employee:
                            EmployeeMenu.ShowEmployeerMenu(ref user);
                            break;

                        default:
                            Console.WriteLine("Invalid role selected.");
                            break;
                    }
                }
                Console.Clear();
                authenticationService.Logout();
            }
        }
    }
}
