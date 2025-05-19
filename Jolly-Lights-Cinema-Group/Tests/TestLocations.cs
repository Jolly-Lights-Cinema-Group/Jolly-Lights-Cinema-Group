using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public static class TestLocations
    {
        public static void CreateTestLocations()
        {
            DatabaseManager.InitializeDatabase();

            List<Location> testLocations = new List<Location>();

            Location locationRotterdam = new Location("Jolly Lights Rotterdam", "Rotterdam Beurs 112");
            Location locationAmsterdam = new Location("Jolly Lights Amsterdam", "Amsterdam Kalverstraat 113");
            Location locationUtrecht = new Location("Jolly Lights Utrecht", "Utrecht Oudegracht 114");

            testLocations.Add(locationRotterdam);
            testLocations.Add(locationAmsterdam);
            testLocations.Add(locationUtrecht);

            LocationRepository locationRepository = new LocationRepository();
            List<Location> locations = locationRepository.GetAllLocations();

            foreach (Location testLocation in testLocations)
            {
                foreach (Location location in locations)
                {
                    if (location.Name == testLocation.Name)
                    {
                        return;
                    }
                    locationRepository.AddLocation(testLocation);
                }
            }

            Console.WriteLine("Test locations added successfully.");
        }
    }
}
