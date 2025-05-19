using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class MovieScheduleTest
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

            // Need to add a Room, movie and location because of foreign keys errors.
            Movie movie = new("test", 90, 1, DateTime.Today, "testcast");
            Location location = new("test", "tes123");

            MovieRepository movieRepository = new MovieRepository();
            MovieRoomRepository movieRoomRepository = new MovieRoomRepository();
            LocationRepository locationRepository = new LocationRepository();

            locationRepository.AddLocation(location);
            movieRepository.AddMovie(movie);
            movieRoomRepository.AddMovieRoom(1, "[a:a]", 1, 1);
        }
        // AddSchedule
        [TestMethod]
        public void Test_Addschedule_AddscheduletoDB()
        {
            Schedule schedule = new Schedule(1, 1, DateTime.Today, TimeSpan.FromMinutes(60));

            ScheduleRepository scheduleRepository = new ScheduleRepository();

            bool result = scheduleRepository.AddSchedule(schedule);

            Assert.IsTrue(result, "AddSchedule should return true if insertion was successful.");
        }

        // DeleteScheduleLine
        [TestMethod]
        public void Test_DeleteSchedule_DeleteScheduletoDB()
        {

            Schedule schedule = new Schedule(1, 1, DateTime.Today, TimeSpan.FromMinutes(60));
            ScheduleRepository scheduleRepository = new ScheduleRepository();
            scheduleRepository.AddSchedule(schedule);

            bool result = scheduleRepository.DeleteScheduleLine(schedule);
            Assert.IsTrue(result, "DeleteSchedule should return true if insertion was successful.");
        }
        // ShowSchedule
        [TestMethod]
        [DoNotParallelize]
        public void Test_ShowSchedule_ShowFullScheduleDB()
        {
            Schedule schedule1 = new Schedule(1, 1, DateTime.Today, TimeSpan.FromMinutes(60));
            Schedule schedule2 = new Schedule(1, 1, DateTime.Today, TimeSpan.FromMinutes(90));
            Schedule schedule3 = new Schedule(1, 1, DateTime.Today, TimeSpan.FromMinutes(120));

            ScheduleRepository scheduleRepository = new ScheduleRepository();

            scheduleRepository.AddSchedule(schedule1);
            scheduleRepository.AddSchedule(schedule2);
            scheduleRepository.AddSchedule(schedule3);

            List<Schedule> Fullschedule = scheduleRepository.GetAllSchedules();

            bool? ScheduleTest1 = false;
            bool? ScheduleTest2 = false;
            bool? ScheduleTest3 = false;

            foreach (Schedule sch in Fullschedule)
            {
                if (sch.StartTime == schedule1.StartTime) { ScheduleTest1 = true; }
                if (sch.StartTime == schedule1.StartTime) { ScheduleTest2 = true; }
                if (sch.StartTime == schedule1.StartTime) { ScheduleTest3 = true; }
            }

            Assert.IsTrue(ScheduleTest1, $"TimeSpan: ScheduleTest1  should have been detected.");
            Assert.IsTrue(ScheduleTest2, $"TimeSpan: ScheduleTest2  should have been detected.");
            Assert.IsTrue(ScheduleTest3, $"TimeSpan: ScheduleTest3  should have been detected.");
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