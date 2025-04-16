using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public static class LocationService
{
    public static void RegisterLocation(Location location)
    {
        if (LocationRepository.AddLocation(location))
        {
            Console.WriteLine("Location added successfully.");
            return;
        }

        Console.WriteLine("Location was not added to the database.");
        return;
    }

    public static void DeleteLocation(Location location)
    {
        if (LocationRepository.RemoveLocation(location))
        {
            Console.WriteLine("Location removed successfully.");
            return;
        }
        Console.WriteLine("No matching location found to remove.");
        return;
    }

    public static void ShowAllLocations()
    {
        List<Location> locations = LocationRepository.GetAllLocations();
        if (locations.Count == 0)
        {
            Console.WriteLine("No locations found.");
            return;
        }

        Console.WriteLine("Locations:");
        foreach (var location in locations)
        {
            Console.WriteLine($"Name: {location.Name}; Address: {location.Address}");
        }
        return;
    }
    public static void UpdateLocation(Location location, string? newName, string? newAddress)
    {
        if (LocationRepository.ModifyLocation(location, newName, newAddress))
        {
            Console.WriteLine("Location is updated");
            return;
        }
        Console.WriteLine("No location found to update.");
        return;
    }
}
