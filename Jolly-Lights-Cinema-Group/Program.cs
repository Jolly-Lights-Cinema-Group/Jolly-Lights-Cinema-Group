using Jolly_Lights_Cinema_Group.Domain;

namespace Jolly_Lights_Cinema_Group
{
    class Program
    {
        static void Main()
        {
            // Main menu / Asking for role
            string prompt = "Jolly Lights Cinema Group";
            string[] options = { "Employee", "Manager", "Admin" };
            Menu menu = new(prompt, options);
            int selectedIndex = menu.Run();
            
            // Main menu / Asking for location
            string locationPrompt = "Choose a location";
            string[] optionsLocation = { "Rotterdam", "Utrecht", "Amsterdam" };
            LocationMenu location = new(locationPrompt, optionsLocation);
            int selectedLocation = location.Run();

            
            // Some login code to be developed.   -- Very basic authentication here. Will work with this if we do have the authentication method.
            string Username = string.Empty;
            string Password = string.Empty;
            do
            {
            Console.WriteLine("Login.");
            Console.WriteLine("Username: ");
            Username = Console.ReadLine()!;
            Console.WriteLine("Password: ");
            Password = Console.ReadLine()!;
            } while (Username != "Admin" || Password != "Admin");
            
            User user = new(options[selectedIndex], optionsLocation[selectedLocation], true);
            Console.Clear();
            Console.WriteLine($"Login successfull.\n\nWelcome {Username}!\nRole - {user.Role}\nLocation - {user.Location}");
            Console.ReadKey();


            while (user.IsAuthenticated)
        {
            switch (user.Role)
            {
                case "Admin":
                    AdminMenu adminMenu = new AdminMenu();
                    int adminChoice = adminMenu.Run();
                    AdminChoiceHandler.HandleChoice(adminChoice, ref user);
                    break;

                case "Manager":
                    ManagerMenu managerMenu = new ManagerMenu();
                    int managerChoice = managerMenu.Run();
                    managerChoiceHandler.HandleChoice(managerChoice, ref user);
                    break;

                case "Employee":
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
        Console.WriteLine($"Succesfully logged out.\nSee you next time {Username}");  
        }
    }
}