namespace Jolly_Lights_Cinema_Group
{
    public static class AdminLocationHandler
    {
        public static void ManageLocations()
        {
            bool inManageLocationMenu = true;
            AdminManageLocationsMenu manageLocationsMenu = new();
            Console.Clear();
            
            while(inManageLocationMenu)
            {
            int userChoice = manageLocationsMenu.Run();
            inManageLocationMenu = HandleManageLocationsChoice(userChoice);
            Console.Clear();
            }
        }
        private static bool HandleManageLocationsChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    AddLocation();
                    return true;
                case 1:
                    DeleteLocation();
                    return true;
                case 2:
                    ViewAllLocations();
                    return true;
                case 3:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }
        public static void AddLocation()
        {
            Console.Clear();

            Console.WriteLine("Information to add location:");
            Console.Write("Enter the name of the location: ");
            string? name = Console.ReadLine();

            Console.Write("Enter the address of the location: ");
            string? address = Console.ReadLine();


            LocationService locationService = new LocationService();
            if(name != null && address != null)
            {
                Location location = new(name, address);
                locationService.RegisterLocation(location);
            }
            else
            {
                Console.WriteLine("Invalid location");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }


        public static void DeleteLocation()
        {
            Console.Clear();

            Console.WriteLine("Information to delete location:");

            Console.Write("Enter the name of the location: ");
            string? name = Console.ReadLine();

            Console.Write("Enter the address of the location: ");
            string? address = Console.ReadLine();

            LocationService locationService = new LocationService();
            if(name != null && address != null)
            {
                Location location = new(name, address);
                locationService.DeleteLocation(location);
            }
            else
            {
                Console.WriteLine("Invalid location");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        public static void ViewAllLocations()
        {
            Console.Clear();
            LocationService locationService = new LocationService();
            locationService.ShowAllLocations();      
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}