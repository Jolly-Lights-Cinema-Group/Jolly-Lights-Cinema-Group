// using System.Runtime.Intrinsics.Arm;
// using JollyLightsCinemaGroup.DataAccess;

// namespace Jolly_Lights.Tests
// {
//     [TestClass]
//     public class ShopItemTests
//     {
//         private string? _tempDir;

//         [TestInitialize]
//         public void Setup()
//         {
//             _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
//             Directory.CreateDirectory(_tempDir);

//             string testSchemaPath = Path.Combine(_tempDir, "schema.sql");

//             var originalSchemaPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Jolly-Lights-Cinema-Group", "Database", "schema.sql");
//             File.Copy(originalSchemaPath, testSchemaPath);

//             DatabaseManager.OverridePaths(_tempDir);

//             DatabaseManager.InitializeDatabase();
//         }

//         [TestMethod]
//         public void Test_AddShopItem()
//         {
//             ShopItem shopItem = new("Cola", 2, 30);

//             ShopItemRepository shopItemRepository = new ShopItemRepository();

//             bool result = shopItemRepository.AddShopItem(shopItem);

//             Assert.IsTrue(result, "ShopItem not added to database");
//         }

//         [TestMethod]
//         public void Test_GetAllShopItems()
//         {

//             ShopItem shopItem1 = new("Lays Naturel", 2, 30);
//             ShopItem shopItem2 = new("3D glasses", 3, 30);

//             ShopItemRepository shopItemRepository = new ShopItemRepository();

//             shopItemRepository.AddShopItem(shopItem1);
//             shopItemRepository.AddShopItem(shopItem2);

//             List<ShopItem> shopItems = shopItemRepository.GetAllShopItems();

//             bool resultShopItem1 = false;
//             bool resultShopItem2 = false;
//             foreach (ShopItem shopItem in shopItems)
//             {
//                 if (shopItem.Name == shopItem1.Name)
//                 {
//                     resultShopItem1 = true;
//                 }
//                 if (shopItem.Name == shopItem2.Name)
//                 {
//                     resultShopItem2 = true;
//                 }
//             }

//             Assert.IsTrue(resultShopItem1, "ShopItem 1 not findable in shopitems");
//             Assert.IsTrue(resultShopItem2, "ShopItem 2 not findable in shopitems");
//         }

//         [TestMethod]
//         public void Test_ModifyShopItem()
//         {
//             ShopItem oldShopItem = new("Lays Paprika", 2, 30);

//             ShopItemRepository shopItemRepository = new ShopItemRepository();
//             shopItemRepository.AddShopItem(oldShopItem);
//             ShopItem shopItemId = shopItemRepository.GetShopItemByName(oldShopItem.Name)!;

//             bool result = shopItemRepository.ModifyShopItem(shopItemId, "", "3", "", "18");
//             Assert.IsTrue(result, "ShopItem could not be modified.");
//         }

//         [TestCleanup]
//         public void Cleanup()
//         {
//             if (_tempDir != null && Directory.Exists(_tempDir))
//             {
//                 for (int i = 0; i < 5; i++)
//                 {
//                     try
//                     {
//                         Directory.Delete(_tempDir, recursive: true);
//                         break;
//                     }
//                     catch (IOException)
//                     {
//                         Thread.Sleep(1); // If cinemaDB is still busy, then wait 1ms and try again
//                     }
//                 }
//             }
//         }
//     }
// }
