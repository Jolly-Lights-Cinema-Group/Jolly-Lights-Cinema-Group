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
        if (string.IsNullOrWhiteSpace(location.Name))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(location.Address))
        {
            Console.WriteLine("Error: Address cannot be empty.");
            return;
        }

        _locationRepo.AddLocation(location);
    }

    public void DeleteLocation(Location location)
    {
        if (string.IsNullOrWhiteSpace(location.Name))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(location.Address))
        {
            Console.WriteLine("Error: Address cannot be empty.");
            return;
        }

        _locationRepo.RemoveLocation(location);
    }

    public void ShowAllLocations()
    {
        List<string> locations = _locationRepo.GetAllLocations();
        if (locations.Count == 0)
        {
            Console.WriteLine("No locations found.");
        }
        else
        {
            Console.WriteLine("Locations:");
            foreach (var location in locations)
            {
                Console.WriteLine(location);
            }
        }
    }
}
