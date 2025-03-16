namespace Jolly_Lights_Cinema_Group
{
    class Program
    {
        static void Main()
        {
            // some login code to be developed.
            
            // some location choice menu to be developed.
            
            // Main menu
            string prompt = "Jolly Lights Cinema Group";
            string[] options = { "Medewerker", "Manager", "Admin" };
            Menu menu = new Menu(prompt, options);
            string locationPrompt = "Choose a location";
            string[] optionsLocation = { "Rotterdam", "Utrecht", "Amsterdam" };
            Location location = new Location(locationPrompt, optionsLocation);
            int selectedIndex = menu.Run();
            int selectedLocation = location.Run();
        }
    }
}