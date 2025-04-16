using Jolly_Lights_Cinema_Group;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public class LocationMenu : Menu
    {
        public LocationMenu() : base("Choose a location", GetLocationOptions()) { }

        private static string[] GetLocationOptions()
        {
            List<Location> locations = LocationRepository.GetAllLocations();
            return locations.Select(location => location.Name).ToArray();
        }
    }
}
