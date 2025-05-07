using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class ScheduleShopItemTests
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
        public void Test_AddScheduleShopItem()
        {
            ShopItem shopItem = new("Nachos M", 4, 6);
            ShopItemRepository shopItemRepository = new ShopItemRepository();
            shopItemRepository.AddShopItem(shopItem);
            ShopItem shopItemId = shopItemRepository.GetShopItemByName(shopItem.Name)!;

            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();
            Reservation reservation = new("Pieter", "Jan", 12345, "pieter.jan@gmail.com", reservationNumber);
            ReservationRepository reservationRepository = new();
            reservationRepository.AddReservation(reservation);
            Reservation reservationId = reservationRepository.FindReservationByReservationNumber(reservationNumber)!;

            ScheduleShopItem scheduleShopItem = new(shopItemId.Id!.Value, reservationId.Id!.Value);
            ScheduleShopItemRepository scheduleShopItemRepository = new();
            bool result = scheduleShopItemRepository.AddScheduleShopItem(scheduleShopItem);

            Assert.IsTrue(result, "ScheduledShopItem not added to database");
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
