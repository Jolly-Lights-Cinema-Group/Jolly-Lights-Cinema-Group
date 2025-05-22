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
            movieRoomRepository.AddMovieRoom(1, "[a:a]", 1, 1);
            string firstName = "Jane";
            string lastName = "Doe";
            int phoneNumber = 123456789;
            string eMail = "jane.doe@gmail.com";
            string reservationNumber = "1";

            OrderLine orderline = new OrderLine(1, 1, "1", 1, 1.11);
            Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);

            ReservationRepository reservationRepository = new ReservationRepository();
            OrderLineRepository orderLineRepository = new OrderLineRepository();

            reservationRepository.AddReservation(reservation);
            orderLineRepository.AddOrderLine(orderline);
        }

        [TestMethod]
        public void Test_DiscountCode_CheckIfCodeExist()
        {
            DiscountCode discountcode = new DiscountCode("1", 0.2, "Compensation", DateTime.Now.AddYears(1), null);
            DiscountCodeRepository discountCodeRepository = new DiscountCodeRepository();

            discountCodeRepository.AddDiscountCode(discountcode);

            bool result = discountCodeRepository.CheckIfCodeExist(discountcode.Code);

            Assert.IsTrue(result, "Code should be in the DB. Should return true if insertion was successfull.");

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

            bool result = discountCodeRepository.DeleteDiscountCode(discountcode.Code);

            Assert.IsTrue(result, "Code should be in the DB. Should have been deleted.");
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