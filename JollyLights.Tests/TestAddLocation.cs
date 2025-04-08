using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Models;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class LocationTests
    {
        [TestInitialize]
        public void Setup()
        {
            DatabaseManager.InitializeDatabase();
        }

        [TestMethod]
        public void Test_AddLocation_AddsLocationToDatabase()
        {
            string name = "TestLocation";
            string address = "TestStreet 123";
            Location location = new(name, address);
            LocationService locationService = new LocationService();

            bool result = locationService.RegisterLocation(location);

            Assert.IsTrue(result, "Location added successfully.");
        }
    }
}
