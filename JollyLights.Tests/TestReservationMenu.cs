using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class ReservationTests
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
        public void Test_AddReservation()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            int phoneNumber = 123456789;
            string eMail = "jane.doe@gmail.com";
            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();

            Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);

            ReservationRepository reservationRepository = new ReservationRepository();
            bool result = reservationRepository.AddReservation(reservation);

            Assert.IsTrue(result, "Reservation not added to database");
        }

        [TestMethod]
        public void Test_RemoveReservation()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            int phoneNumber = 123456789;
            string eMail = "jane.doe@gmail.com";
            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();

            Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);

            ReservationRepository reservationRepository = new ReservationRepository();

            reservationRepository.AddReservation(reservation);
            Reservation? newReservation = reservationRepository.FindReservationByReservationNumber(reservation.ReservationNumber);
            bool result = reservationRepository.RemoveReservation(newReservation!);

            Assert.IsTrue(result, "Reservation not removed from database");
        }

        [TestMethod]
        public void Test_GetAllReservations()
        {
            Reservation reservation1 = new Reservation("Jane", "Doe", 123456789, "jane.doe@gmail.com", ReservationNumberGenerator.GetReservationNumber());
            Reservation reservation2 = new Reservation("John", "Doe", 123456789, "john.doe@gmail.com", ReservationNumberGenerator.GetReservationNumber());
            Reservation reservation3 = new Reservation("Piet", "Heijn", 123456789, "piet.heijn@gmail.com", ReservationNumberGenerator.GetReservationNumber());

            ReservationRepository reservationRepository = new ReservationRepository();

            reservationRepository.AddReservation(reservation1);
            reservationRepository.AddReservation(reservation2);
            reservationRepository.AddReservation(reservation3);

            List<Reservation> reservations = reservationRepository.GetAllReservations();

            bool reservationTest1 = false;
            bool reservationTest2 = false;
            bool reservationTest3 = false;

            foreach (Reservation reservation in reservations)
            {
                if (reservation.ReservationNumber == reservation1.ReservationNumber) { reservationTest1 = true; }
                if (reservation.ReservationNumber == reservation2.ReservationNumber) { reservationTest2 = true; }
                if (reservation.ReservationNumber == reservation3.ReservationNumber) { reservationTest3 = true; }
            }

            Assert.IsTrue(reservationTest1, $"Reservation number: {reservation1.ReservationNumber} was not detected");
            Assert.IsTrue(reservationTest2, $"Reservation number: {reservation2.ReservationNumber} was not detected");
            Assert.IsTrue(reservationTest3, $"Reservation number: {reservation3.ReservationNumber} was not detected");
        }

        [TestMethod]
        public void Test_FindReservationByReservationNumber()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            int phoneNumber = 123456789;
            string eMail = "jane.doe@gmail.com";
            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();

            Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);

            ReservationRepository reservationRepository = new ReservationRepository();
            reservationRepository.AddReservation(reservation);

            Reservation? getReservation = reservationRepository.FindReservationByReservationNumber(reservation.ReservationNumber);
            Assert.IsNotNull(getReservation, "No reservation was retrieved");
            Assert.AreEqual(reservation.ReservationNumber, getReservation.ReservationNumber);
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
