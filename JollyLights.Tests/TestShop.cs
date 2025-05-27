using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class ShopTests
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
        public void TestShop()
        {
            ShopItem shopItem = new("Nachos L", 8, 6);
            ShopItemRepository shopItemRepository = new ShopItemRepository();
            shopItemRepository.AddShopItem(shopItem);
            ShopItem shopItemId = shopItemRepository.GetShopItemByName(shopItem.Name)!;

            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();
            Reservation reservation = new("Pieter", "Jan", 12345, "pieter.jan@gmail.com", reservationNumber);
            ReservationRepository reservationRepository = new();
            reservationRepository.AddReservation(reservation);
            Reservation reservationId = reservationRepository.FindReservationByReservationNumber(reservationNumber)!;

            ShopItemService shopItemService = new();
            shopItemService.SellShopItem(shopItemId, reservationId);

            ShopItem? soldShopItem = shopItemRepository.GetShopItemById(shopItemId.Id!.Value);
            Assert.AreEqual(shopItem.Stock - 1, soldShopItem!.Stock, $"Previous stock: {shopItemId.Stock}; Current stock: {soldShopItem.Stock}");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_tempDir != null && Directory.Exists(_tempDir))
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        Directory.Delete(_tempDir, recursive: true);
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(1); // If cinemaDB is still busy, then wait 1ms and try again
                    }
                }
            }
        }
    }
}