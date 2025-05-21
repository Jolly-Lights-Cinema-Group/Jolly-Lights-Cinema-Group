using Jolly_Lights_Cinema_Group.BusinessLogic;
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
                    return false;
            }
        }
        public static void AddReservation()
        {
            MovieService movieService = new();
            Movie? selectedMovie = movieService.SelectMovieMenu();

            if (selectedMovie is null) return;

            ScheduleService scheduleService = new();
            Schedule? selectedSchedule = scheduleService.SelectScheduleMenu(selectedMovie);

            if (selectedSchedule is null) return;

            MovieRoomRepository movieRoomRepository = new();
            MovieRoom? movieRoom = movieRoomRepository.GetMovieRoomById(selectedSchedule.MovieRoomId);

            if (movieRoom is null) return;
 
            var locationId = movieRoom.LocationId;
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

            var seatRepo = new SeatRepository();
            var seatPrice = seatRepo.GetSeatPriceForSeatTypeOnLocation(seatType ,locationId);

            Reservation? reservation = reservationService.MakeReservation();
            if (reservation is null) return;

            ScheduleSeatRepository scheduleSeatRepository = new();

            if (reservation.Id != null)
                scheduleSeatRepository.AddSeatToReservation(selectedSeat, seatType, (int)reservation.Id, (int)scheduleId!, seatPrice);

            reservationService.CompleteReservation(reservation, selectedMovie, selectedSchedule, selectedSeat);
        }

        public static void EditReservation()
        {
            string? reservationNumber;
            do
            {
                Console.Write("Enter reservation number to edit reservation: ");
                reservationNumber = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(reservationNumber));


            ReservationRepository reservationRepository = new ReservationRepository();
            Reservation? reservation = reservationRepository.FindReservationByReservationNumber(reservationNumber);

            if (reservation != null)
            {
                Menu editReservationMenu = new($"Edit reservation: {reservation.ReservationNumber}", new string[] { "Edit Movie", "Edit schedule", "Edit shop items", "Delete reservation" });

            }
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
                OrderLineRepository orderLineRepository = new();
                List<OrderLine> orderLines = orderLineRepository.GetOrderLinesByReservation(reservation);

                foreach (OrderLine orderLine in orderLines)
                {
                    Console.WriteLine($"{orderLine.Description} * {orderLine.Quantity} = €{orderLine.Price}     ({orderLine.VatPercentage}% VAT)");
                }
                Console.WriteLine($"-----------------------------------------------------------------------");
                Console.WriteLine($"Subtotal (excl. Tax): €{Math.Round(customerOrder.GrandPrice - customerOrder.Tax, 2)}");
                Console.WriteLine($"VAT: €{customerOrder.Tax}");
                Console.WriteLine($"Total (incl. Tax): €{customerOrder.GrandPrice}");

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