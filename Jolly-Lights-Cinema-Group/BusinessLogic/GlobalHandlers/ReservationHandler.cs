using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ReservationHandler
    {
        private static int SelectedIndexY;
        private static int SelectedIndexX;

        
        public static void ManageReservations()
        {
            var inManageReservationsMenu = true;
            ReservationMenu reservationMenu = new();
            Console.Clear();
            
            while(inManageReservationsMenu)
            {
                var userChoice = reservationMenu.Run();
                inManageReservationsMenu = HandleManageReservationsChoice(userChoice);
                Console.Clear();
            }
        }
        private static bool HandleManageReservationsChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    AddReservation();
                    return true;
                case 1:
                    DeleteReservation();
                    return true;
                case 2:
                    PayReservation();
                    return true;
                case 3:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }
        public static void AddReservation()
        {
            Console.Clear();
            Console.WriteLine("Add Reservation:");

            ScheduleService scheduleService = new();
            List<Movie> scheduledMovies = scheduleService.GetMoviesBySchedule();

            string[] movieMenuItems = scheduledMovies
                .Select(movie => $"Movie: {movie.Title}; Duration: {movie.Duration} minutes; Min Age: {movie.MinimumAge}")
                .Append("Cancel")
                .ToArray();

            Menu movieMenu = new("Select a movie:", movieMenuItems);
            int movieChoice = movieMenu.Run();

            if (movieChoice >= scheduledMovies.Count)
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            Movie selectedMovie = scheduledMovies[movieChoice];

            ScheduleRepository scheduleRepository = new();
            List<Schedule> schedules = scheduleRepository.GetSchedulesByMovie(selectedMovie);

            var groupedSchedules = schedules
                .GroupBy(s => s.StartDate.Date)
                .OrderBy(g => g.Key)
                .ToList();

            Schedule? selectedSchedule = null;

            while (true)
            {
                string[] dateMenuItems = groupedSchedules
                    .Select(g => g.Key.ToString("dddd dd MMMM yyyy"))
                    .Append("Cancel")
                    .ToArray();

                Menu dateMenu = new("Choose a date:", dateMenuItems);
                int dateChoice = dateMenu.Run();

                if (dateChoice == groupedSchedules.Count)
                {
                    Console.WriteLine("Canceled.");
                    return;
                }

                var selectedDateGroup = groupedSchedules[dateChoice];

                List<Schedule> timesForDate = selectedDateGroup.OrderBy(s => s.StartTime).ToList();

                string[] timeMenuItems = timesForDate
                    .Select(s => s.StartTime.ToString(@"hh\:mm"))
                    .Append("Go back to dates")
                    .ToArray();

                Menu timeMenu = new("Choose a time:", timeMenuItems);
                int timeChoice = timeMenu.Run();

                if (timeChoice == timesForDate.Count)
                {
                    continue;
                }

                selectedSchedule = timesForDate[timeChoice];
                break;
            }

            MovieRoomRepository movieRoomRepository = new();
            MovieRoom? movieRoom = movieRoomRepository.GetMovieRoomById(selectedSchedule.MovieRoomId);
            //TODO get id's from selecting movie from schedule.
 
            var locationId = movieRoom!.LocationId;
            var roomId = selectedSchedule.MovieRoomId;
            var scheduleId = selectedSchedule.Id;
            
            MovieRoomService movieRoomService = new MovieRoomService();
            ReservationService reservationService = new ReservationService();
            
            var roomLayout = movieRoomService.GetRoomLayout(roomId, locationId);
            var reservedSeats = reservationService.GetReservedSeats(roomId, locationId);

            var rowCount = 1;
            foreach (var row in roomLayout!)
            {
                var seatCount = 1;
                foreach (var item in row)
                {
                    if (reservedSeats.Contains((rowCount.ToString(), item)))
                    {
                        roomLayout[rowCount][seatCount] = "X";
                    }
                    seatCount++;
                }

                rowCount++;
            }

            do
            {
                StartSeatSelection(roomLayout);
            } while (!(SelectedIndexY < roomLayout.Count &&
                      SelectedIndexY >= 0 &&
                      SelectedIndexX < roomLayout[SelectedIndexY].Count &&
                      SelectedIndexX >= 0) &&
                     !(roomLayout[SelectedIndexY][SelectedIndexX] == "#" ||
                       roomLayout[SelectedIndexY][SelectedIndexX] == "_"));

            Console.Clear();
            var selectedSeat = $"{SelectedIndexY},{SelectedIndexX}";
            var selectedSeatValue = roomLayout[SelectedIndexY][SelectedIndexX];

            var seatType = selectedSeatValue switch
            {
                "S" => SeatType.RegularSeat,
                "L" => SeatType.LoveSeat,
                "P" => SeatType.VipSeat,
                _ => throw new ArgumentOutOfRangeException()
            };

            //TODO get price from selected seat.
            var seatPrice = 10;
            
            string? firstName;
            do
            {
                Console.Write("Enter first name: ");
                firstName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(firstName));

            string? lastName;
            do
            {
                Console.Write("Enter last name: ");
                lastName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(lastName));

            int phoneNumber;
            string? input;
            do
            {
                Console.Write("Enter telephone number: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out phoneNumber) || phoneNumber < 0);

            string? eMail;
            do
            {
                Console.Write("Enter email address: ");
                eMail = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(eMail));
            
            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();

            Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);

            reservationService.RegisterReservation(reservation);

            ReservationRepository reservationRepository = new();
            Reservation newReservation = reservationRepository.FindReservationByReservationNumber(reservation.ReservationNumber)!;

            ScheduleSeatRepository scheduleSeatRepository = new();

            if (newReservation.Id != null)
                scheduleSeatRepository.AddSeatToReservation(selectedSeat, seatType, (int)newReservation.Id, (int)scheduleId!, seatPrice);
            
            Console.Write("\nWould you like to add extra items to your reservation? (y/n): ");
            string? response = Console.ReadLine()?.Trim().ToLower();

            if (response == "y")
            {
                ShopHandler.ManageShop(newReservation);
            }

            OrderLineService orderLineService = new();
            orderLineService.CreateOrderLineForReservation(newReservation);

            Console.WriteLine($"Reservation Number: {newReservation.ReservationNumber}");
            Console.WriteLine($"Movie: {selectedMovie.Title}\nDate: {selectedSchedule.StartDate:dddd dd MMMM yyyy} {selectedSchedule.StartTime:hh\\:mm}");
            Console.WriteLine($"Seat(s): {selectedSeat}");

            Console.WriteLine("\nReservation complete. Press any key to continue.");
            Console.ReadKey();
        }


        public static void DeleteReservation()
        {
            Console.Clear();

            Console.WriteLine("Delete reservation:");

            string? reservationNumber;
            do
            {
                Console.Write("Enter reservation number: ");
                reservationNumber = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(reservationNumber));


            ReservationRepository reservationRepository = new ReservationRepository();
            Reservation? reservation = reservationRepository.FindReservationByReservationNumber(reservationNumber);
            if (reservation != null)
            {
                ReservationService reservationService = new ReservationService();
                reservationService.DeleteReservation(reservation);
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        public static void PayReservation()
        {
            Console.Clear();

            string? reservationNumber;
            do
            {
                Console.Write("Enter reservation number: ");
                reservationNumber = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(reservationNumber));

            ReservationRepository reservationRepository = new();
            Reservation reservation = reservationRepository.FindReservationByReservationNumber(reservationNumber)!;


            CustomerOrderService customerOrderService = new();
            CustomerOrder customerOrder = customerOrderService.CreateCustomerOrderForReservation(reservation);

            ReservationService reservationService = new();

            if (!reservationService.IsReservationPaid(reservation))
            {
                Console.WriteLine($"Total: €{customerOrder.GrandPrice}");
                string? input;
                do
                {
                    Console.Write("Confirm payment? (y/n): ");
                    input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "y")
                    {
                        if (reservationService.PayReservation(reservation) && customerOrderService.RegisterCustomerOrder(customerOrder))
                        {
                            Console.WriteLine("Payment confirmed.");
                            break;
                        }
                        Console.WriteLine("Payment could not be confirmed.");
                        break;
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("Payment cancelled.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }

                } while (input != "y" && input != "n");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private static void StartSeatSelection(List<List<string>> seats)
        {
            ConsoleKey keypressed;
            do
            {
                Console.Clear();
                DisplayOptions(seats);

                keypressed = Console.ReadKey(true).Key;

                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (SelectedIndexY > 0) SelectedIndexY--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (SelectedIndexY < seats.Count - 1) SelectedIndexY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (SelectedIndexX > 0) SelectedIndexX--;
                        break;
                    case ConsoleKey.RightArrow:
                        
                        if (SelectedIndexX < seats[SelectedIndexY].Count - 1) SelectedIndexX++;
                        break;
                }

            } while (keypressed != ConsoleKey.Enter);
        }
        
        private static void DisplayOptions(List<List<string>> grid)
        {
            Console.Clear();
            for (var r = 0; r < grid.Count; r++)
            {
                for (var c = 0; c < grid[r].Count; c++)
                {
                    if (r == SelectedIndexY && c == SelectedIndexX)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"[{grid[r][c]}]");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($" {grid[r][c]} ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}