using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public static class TestLocations
    {
        public static void CreateTestLocations()
        {
            DatabaseManager.InitializeDatabase();

            Location locationRotterdam = new Location("Jolly Lights Rotterdam", "Rotterdam Beurs 112");
            Location locationAmsterdam = new Location("Jolly Lights Amsterdam", "Amsterdam Kalverstraat 113");
            Location locationUtrecht = new Location("Jolly Lights Utrecht", "Utrecht Oudegracht 114");

            LocationRepository locationRepository = new LocationRepository();

            locationRepository.AddLocation(locationRotterdam);
            locationRepository.AddLocation(locationAmsterdam);
            locationRepository.AddLocation(locationUtrecht);

            Console.WriteLine("Test locations added successfully.");
        }
    }
}
