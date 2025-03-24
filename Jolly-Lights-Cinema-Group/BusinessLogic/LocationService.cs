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

    public void RegisterLocation(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Error: Name cannot be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("Error: Address cannot be empty.");
            return;
        }

        _locationRepo.AddLocation(name, address);
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
