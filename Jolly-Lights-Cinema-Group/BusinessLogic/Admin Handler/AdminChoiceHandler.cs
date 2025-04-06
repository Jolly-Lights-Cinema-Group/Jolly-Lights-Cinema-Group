

        private static void DeleteLocation()
        {
            Console.Clear();

            Console.WriteLine("Information to delete location:");

            Console.Write("Enter the id of the location: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the name of the location: ");
            string? name = Console.ReadLine();

            Console.Write("Enter the address of the location: ");
            string? address = Console.ReadLine();

            LocationService locationService = new LocationService();
            if(name != null && address != null && id > 0)
            {
                locationService.DeleteLocation(id, name, address);
            }
            else
            {
                Console.WriteLine("Invalid location");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private static void ViewAllLocations()
        {
            Console.Clear();
            LocationService locationService = new LocationService();
            locationService.ShowAllLocations();      
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}