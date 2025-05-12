using System.Data.Common;
using System.Reflection;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class MovieScheduleHandler
    {
        public static void ScheduleMovies()
        {
            bool inScheduleMovies = true;
            ScheduleMenu schedulemenu = new();
            Console.Clear();

            while (inScheduleMovies)
            {
                int userChoice = schedulemenu.Run();
                inScheduleMovies = HandleManageScheduleMenu(userChoice);
                Console.Clear();
            }
        }
        private static bool HandleManageScheduleMenu(int choice)
        {
            switch (choice)
            {
                case 0:
                    AddScheduleline();
                    return true;
                case 1:
                    Console.WriteLine("Choice2");
                    return true;
                case 2:
                    return true;
                case 3:
                    return true;
                case 4:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }

        // adding movie schedule (making)  -- Movieroom (based on ID),moviename (based on id),startdate
        private static void AddScheduleline()
        {
            Console.Clear();
            // Testdata for room: (next update has to come from DB)
            MovieRoom TestDataForMovieRoom = new(id: 1, roomNumber: 1, roomLayoutJson: "aa", supportedMovieType: 1, locationId: 1);

            ScheduleService scheduleservice = new();

            // Adding MovieId
            Movie? selectedMovie = null;
            do
            {
                Console.WriteLine("From the movie DB, What movie do you want to add? (Title or MovieId)");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int movieId))
                {
                    selectedMovie = MovieRepository.GetMovieById(movieId);
                    if (selectedMovie != null)
                    {
                        Console.WriteLine($"Movie selected: {selectedMovie.Title}");
                    }
                    else
                    {
                    }
                }
                else if (!string.IsNullOrWhiteSpace(input))
                {
                    selectedMovie = MovieRepository.GetMovieByTitle(input);
                    if (selectedMovie != null)
                    {
                        Console.WriteLine($"Movie selected: {selectedMovie.Title}");
                    }
                    else
                    {
                        Console.WriteLine("Movie not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid movie title or ID.");
                }
            } while (selectedMovie == null);


            // Adding RoomNumber
            int roomNumber = -1;
            do
            {
                Console.WriteLine("In what room will it play? (Index of available rooms)");
                string? roomInput = Console.ReadLine();
                if (int.TryParse(roomInput, out roomNumber))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid room number.");
                }
            } while (roomNumber < 0);

            // Adding StartDate
            DateTime startDate;
            do
            {
                Console.WriteLine("What is the start date? (dd/MM/yyyy)");
                string? inputDate = Console.ReadLine();
                if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out startDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use dd/MM/yyyy (e.g., 09/05/2025).");
                }
            } while (true);

            // Adding StartTime
            TimeSpan startTime;
            do
            {
                Console.WriteLine("What is the start time? (HH:mm:ss)");
                string? inputTime = Console.ReadLine();
                if (TimeSpan.TryParseExact(inputTime, "hh\\:mm\\:ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    out startTime))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please use HH:mm:ss (e.g., 14:30:00).");
                }
            } while (true);

            Schedule schedule = new(roomNumber, selectedMovie.Id.Value, startDate, startTime);
            scheduleservice.RegisterSchedule(schedule);
            Console.ReadKey();
        }


        // deleting schedule (delete)

        // show schedule

        // Automatic schedule


    }
}