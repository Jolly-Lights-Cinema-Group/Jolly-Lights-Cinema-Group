using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class LocationService
{
    private readonly LocationRepository _locationRepository = new LocationRepository();
    public bool RegisterLocation(Location location)
    {
        return _locationRepository.AddLocation(location);
    }

    public bool DeleteLocation(Location location)
    {
        return _locationRepository.RemoveLocation(location);
    }

    public List<Location> GetAllLocations()
    {
        return _locationRepository.GetAllLocations();
    }
    public bool UpdateLocation(Location location, string? newName, string? newAddress)
    {
        return _locationRepository.ModifyLocation(location, newName, newAddress);
    }

    public Location? GetLocation(Location location)
    {
        return _locationRepository.GetLocation(location);
    }
}
