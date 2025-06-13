
using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;


namespace Jolly_Lights.Tests
{
    [TestClass]
    public class DiscountCodeTest
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

            // Have to add orderline and reservation because of foreign key constrains.

            Movie movie = new("test", 90, 1, DateTime.Today, "testcast");
            Location location = new("test", "tes123");

            MovieRepository movieRepository = new MovieRepository();
            MovieRoomRepository movieRoomRepository = new MovieRoomRepository();
            LocationRepository locationRepository = new LocationRepository();

            locationRepository.AddLocation(location);
            movieRepository.AddMovie(movie);
            MovieRoom movieRoom = new(1, "[a:a]", MovieType.Regular, 1);
            movieRoomRepository.AddMovieRoom(movieRoom);
            string firstName = "Jane";
            string lastName = "Doe";
            int phoneNumber = 123456789;
            string eMail = "jane.doe@gmail.com";
            string reservationNumber = "1234567890";

            OrderLine orderline = new OrderLine(1, 1, "1", 1, 1.11);
            Reservation reservation = new(1, firstName, lastName, phoneNumber, eMail, reservationNumber, false);

            ReservationRepository reservationRepository = new ReservationRepository();
            OrderLineRepository orderLineRepository = new OrderLineRepository();

            reservationRepository.AddReservation(reservation);
            orderLineRepository.AddOrderLine(orderline);
        }

        [TestMethod]
        public void CheckIfCodeExist_ShouldReturnTrue_WhenCodeWasAdded()
        {
            DiscountCode discountcode = new DiscountCode("1", 0.2, "Compensation", DateTime.Now.AddYears(1), null);
            DiscountCodeRepository discountCodeRepository = new DiscountCodeRepository();

            discountCodeRepository.AddDiscountCode(discountcode);

            bool result = discountCodeRepository.CheckIfCodeExist(discountcode.Code!);

            Assert.IsTrue(result, "Should return true if insertion was successfull.");

        }

        [TestMethod]
        public void Test_DiscountCode_AddDiscountCode()
        {
            DiscountCode discountcode = new DiscountCode("1", 0.2, "Compensation", DateTime.Now.AddYears(1), null);
            DiscountCodeRepository discountCodeRepository = new DiscountCodeRepository();

            bool result = discountCodeRepository.AddDiscountCode(discountcode);

            Assert.IsTrue(result, "Code should be in the DB. Should have been added.");

        }

        [TestMethod]
        public void Test_DiscountCode_DeleteDiscountCode()
        {
            DiscountCode discountcode = new DiscountCode("1", 0.2, "Compensation", DateTime.Now.AddYears(1), null);
            DiscountCodeRepository discountCodeRepository = new DiscountCodeRepository();

            discountCodeRepository.AddDiscountCode(discountcode);

            bool result = discountCodeRepository.DeleteDiscountCode(discountcode.Code!);

            Assert.IsTrue(result, "Code should be in the DB. Should have been deleted.");
        }

        [TestMethod]
        public void CreateWithCompensationDiscountCode_ShouldReturnValidDiscountCode()
        {
            // Act
            var discountCode = DiscountCode.CreateWithCompensationDiscountCode();

            // Assert
            Assert.IsNotNull(discountCode);
            Assert.AreEqual(0.2, discountCode.DiscountAmount);
            Assert.AreEqual("Compensation", discountCode.DiscountType);
            Assert.IsTrue(discountCode.ExperationDate > DateTime.Now);
            Assert.IsFalse(discountCode.ExperationDate < DateTime.Now);
        }

        [TestMethod]
        public void CreateWithCompensationDiscountCode_ShouldContainOnlyAllowedCharacters()
        {
            string allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            var discountCode = DiscountCode.CreateWithCompensationDiscountCode();

            Assert.IsNotNull(discountCode);
            foreach (char c in discountCode.Code)
            {
                Assert.IsTrue(allowedChars.Contains(c), $"Character '{c}' is not in the allowed characters set.");
            }
        }

        [TestMethod]
        [DataRow(100)]
        [DataRow(200)]
        [DataRow(500)]
        [DataRow(1000)]
        public void CalculateWithCompensationDiscountCode_ShouldBeTwentyProcentOffDataRow(int input)
        {
            var discountCode = DiscountCode.CreateWithCompensationDiscountCode();
            double expected = input - (input * 0.2);
            double actual = input - (input * discountCode.DiscountAmount);

            Assert.AreEqual(expected, actual, $"The expected: {expected} and actual: {actual} are not the same.");
        }

        // Cleanup
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
                        Thread.Sleep(1);
                    }
                }
            }
        }


    }

}