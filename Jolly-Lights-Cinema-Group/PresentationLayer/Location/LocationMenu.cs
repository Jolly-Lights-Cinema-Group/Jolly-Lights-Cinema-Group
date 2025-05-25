using Jolly_Lights_Cinema_Group;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public class LocationMenu : Menu
    {
        public LocationMenu() : base("Choose a location", GetLocationOptions()) { }

        private static string[] GetLocationOptions()
        {
            LocationRepository locationRepository = new LocationRepository();
            List<Location> locations = locationRepository.GetAllLocations();
            return locations.Select(location => location.Name).ToArray();
        }
    }
}
