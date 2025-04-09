using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class LocationService
{
    private readonly LocationRepository _locationRepo;

    public LocationService()
    {
        _locationRepo = new LocationRepository();
    }

    public void RegisterLocation(Location location)
    {
        if (_locationRepo.AddLocation(location))
        {
            Console.WriteLine("Location added successfully.");
            return;
        }

        Console.WriteLine("Location was not added to the database.");
        return;
    }

    public void DeleteLocation(Location location)
    {
        if (_locationRepo.RemoveLocation(location))
        {
            Console.WriteLine("Location removed successfully.");
            return;
        }
        Console.WriteLine("No matching location found to remove.");
        return;
    }

    public void ShowAllLocations()
    {
        List<Location> locations = _locationRepo.GetAllLocations();
        if (locations.Count == 0)
        {
            Console.WriteLine("No locations found.");
        }
        else
        {
            Console.WriteLine("Locations:");
            foreach (var location in locations)
            {
                Console.WriteLine($"Name: {location.Name}; Address: {location.Address}");
            }
        }
    }
}
