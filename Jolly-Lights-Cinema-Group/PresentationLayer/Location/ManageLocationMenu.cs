using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;

public static class ManageLocationMenu
{
    private static LocationService _locationService = new();
    private static Menu _manageLocationMenu = new("Location Management Menu.", new string[] { "Add location", "Delete lcoation", "View locations", "Modify location", "Back" });
    public static void ShowManageLocationMenu()
    {
        bool inManageLocactionMenu = true;
        Console.Clear();

        while (inManageLocactionMenu)
        {
            int userChoice = _manageLocationMenu.Run();
            inManageLocactionMenu = HandleManageLocationChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleManageLocationChoice(int choice)
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
                EditLocation();
                return true;
            case 4:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    private static void AddLocation()
    {
        Console.Clear();

        List<Location> locations = _locationService.GetAllLocations();

        Console.WriteLine("Information to add location:");
        string? name;
        do
        {
            Console.Write("Enter the name of the location: ");
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));

        string? address;
        do
        {
            Console.Write("Enter the address of the location: ");
            address = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(address));

        if (locations.Any(location => location.Address == address))
        {
            Console.WriteLine($"Location with address: {address}, already exists");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            return;
        }

        Location location = new(name, address);

        Console.Clear();
        ;
        if (_locationService.RegisterLocation(location))
        {
            Console.WriteLine("Location added successfully.");
        }
        else Console.WriteLine("Location was not added to the database.");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }
    public static void DeleteLocation()
    {
        Console.Clear();
        Console.WriteLine("Delete location:");

        string? name;
        do
        {
            Console.Write("Enter the name of the location: ");
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));

        string? address;
        do
        {
            Console.Write("Enter the address of the location: ");
            address = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(address));

        Location location = new(name, address);
        Location? foundLocation = _locationService.GetLocation(location);

        Console.Clear();

        if (foundLocation is null)
        {
            Console.WriteLine($"No location found.");
        }
        else
        {
            Console.WriteLine($"Delete Location:\nName: {foundLocation.Name}, Address: {foundLocation.Address}");
            Console.WriteLine($"\nEnter y to confirm: ");
            string? input = Console.ReadLine();
            if (input != null && input.Trim().ToLower() == "y")
            {
                if (_locationService.DeleteLocation(foundLocation))
                {
                    Console.Clear();
                    Console.WriteLine($"Location deleted succesfully");
                }
                else Console.WriteLine($"Location could not be deleted");
            }

            else
            {
                Console.WriteLine($"Location deletion cancelled");
            }
        }

        Console.WriteLine($"\nPress any key to continue");
        Console.ReadKey();
    }

    public static void ViewAllLocations()
    {
        Console.Clear();
        List<Location> locations = _locationService.GetAllLocations();
        if (locations.Count == 0)
        {
            Console.WriteLine("No locations found.");
        }
        else
        {
            Console.WriteLine("Employees:");
            foreach (Location location in locations)
            {
                Console.WriteLine($"Name: {location.Name}, Address: {location.Address}");
            }
        }
        Console.ReadKey();
    }

    public static void EditLocation()
    {
        Console.Clear();

        Console.WriteLine("Edit location:");

        string? oldName;
        do
        {
            Console.Write("Enter the name of the location you want to edit: ");
            oldName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(oldName));

        string? oldAddress;
        do
        {
            Console.Write("Enter the address of the location you want to edit: ");
            oldAddress = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(oldAddress));

        Location oldLocation = new(oldName, oldAddress);
        Location? oldLocationId = _locationService.GetLocation(oldLocation);
        if (oldLocation is null)
        {
            Console.Write("No location found to edit");
        }

        else
        {
            Console.Write("Enter the new name of the location (leave empty keep the current name): ");
            string? newName = Console.ReadLine();
            Console.Write("Enter the new address of the location (leave empty keep the current address): ");
            string? newAddress = Console.ReadLine();

            if (_locationService.UpdateLocation(oldLocation, newName, newAddress))
            {
                Console.WriteLine("Location is updated");
            }
            else Console.WriteLine("Location could not be updated.");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();   
    }
}