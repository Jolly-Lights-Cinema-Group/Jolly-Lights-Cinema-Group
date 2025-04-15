using System.Security.Cryptography;
using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Models;
using JollyLightsCinemaGroup.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class LocationTests
    {
        private string? _tempDir;

        [TestInitialize]
        public void Setup()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);

            string testSchemaPath = Path.Combine(_tempDir, "schema.sql");     

            var originalSchemaPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Jolly-Lights-Cinema-Group", "Database", "schema.sql");
            File.Copy(originalSchemaPath, testSchemaPath);

            DatabaseManager.OverridePaths(_tempDir);

            DatabaseManager.InitializeDatabase();
        }

        [TestMethod]
        public void Test_AddLocation_AddsLocationToDatabase()
        {
            string name = "TestLocation";
            string address = "TestStreet 123";
            Location location = new(name, address);

            LocationRepository locationRepository = new LocationRepository();

            bool result = locationRepository.AddLocation(location);

            Assert.IsTrue(result, "Location not added to database");
            locationRepository.RemoveLocation(location);
        }

        [TestMethod]
        public void Test_ViewAllLocations()
        {
            LocationRepository locationRepository = new LocationRepository();

            Location location1 = new("TestLocation1", "TestStreet 1");
            Location location2 = new("TestLocation2", "TestStreet 2");

            locationRepository.AddLocation(location1);
            locationRepository.AddLocation(location2);

            List<Location> allLocations = locationRepository.GetAllLocations();

            bool resultLocation1 = false;
            bool resultLocation2 = false;
            foreach (Location location in allLocations)
            {
                if (location.Name == "TestLocation1" && location.Address == "TestStreet 1")
                {
                    resultLocation1 = true;
                }
                if (location.Name == "TestLocation2" && location.Address == "TestStreet 2")
                {
                    resultLocation2 = true;
                }
            }

            Assert.IsTrue(resultLocation1, "Location 1 not vissible in all locations");
            Assert.IsTrue(resultLocation2, "Location 2 not vissible in all locations");

            locationRepository.RemoveLocation(location1);
            locationRepository.RemoveLocation(location2);
        }

        [TestMethod]
        public void Test_DeleteLocation_DeletesLocationFromDatabase()
        {
            string name = "TestLocation";
            string address = "TestStreet 123";
            Location location = new(name, address);

            LocationRepository locationRepository = new LocationRepository();
            locationRepository.AddLocation(location);

            bool result = locationRepository.RemoveLocation(location);

            Assert.IsTrue(result, "Location could not be deleted.");
        }

        [TestMethod]
        public void Test_ModifyLocation_ModifyLocationFromDatabase()
        {
            string name = "TestLocation";
            string address = "TestStreet 123";
            Location oldLocation = new(name, address);
            Location newLocation = new("ModifyLocation", "TestModify 123");

            LocationRepository locationRepository = new LocationRepository();
            locationRepository.AddLocation(oldLocation);

            bool result = locationRepository.ModifyLocation(oldLocation, newLocation.Name, newLocation.Address);

            Assert.IsTrue(result, "Location could not be modified.");

            Location newLocationName = new("ModifyName", "");

            bool resultOnlyModifyName = locationRepository.ModifyLocation(newLocation, newLocationName.Name, newLocationName.Address);

            Assert.IsTrue(resultOnlyModifyName, "Location could not be modified.");

            Location locationToRemove = new(newLocationName.Name, newLocation.Address);
            locationRepository.RemoveLocation(locationToRemove);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, recursive: true);
            }
        }
    }
}
