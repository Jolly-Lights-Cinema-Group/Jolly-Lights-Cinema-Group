using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;
using JollyLightsCinemaGroup.BusinessLogic;

public static class CashDesk
{
    private static Menu _cashDesk = new("Cash Desk", new string[] { "Tickets", "Shop", "Back" });
    public static void ShowCashDeskMenu()
    {
        bool inCashDeskMenu = true;
        Console.Clear();

        while (inCashDeskMenu)
        {
            int userChoice = _cashDesk.Run();
            inCashDeskMenu = HandleCashDeskChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleCashDeskChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                SellTickets();
                return true;
            case 1:
                ShopMenu shopMenu = new();
                shopMenu.CashDeskShop(Globals.SessionLocationId);
                return true;
            case 2:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    public static void SellTickets()
    {
        ScheduleService scheduleService = new();
        MovieRoomService movieRoomService = new();
        MovieService movieService = new();
        ScheduleSeatService scheduleSeatService = new();

        DateTime today = DateTime.Now;
        List<Schedule> schedules = scheduleService.ShowScheduleByDate(DateTime.Now, Globals.SessionLocationId);

        List<Schedule> upcomingSchedules = schedules
            .Where(s =>
            {
                DateTime fullStartDateTime = s.StartDate.Date.Add(s.StartTime);
                return fullStartDateTime >= today && movieRoomService.GetLeftOverSeats(s) > 0;
            })
            .OrderBy(s => s.StartDate.Date.Add(s.StartTime))
            .ToList();
        
        List<DateTime>? birthDates = null;

        while (true)
        {
            string[] menuItems = upcomingSchedules
                .Select(s =>
                {
                    Movie movie = movieService.GetMovieById(s.MovieId)!;
                    DateTime start = s.StartDate.Date.Add(s.StartTime);
                    return $"{movie.Title} — {start:HH:mm}";
                })
                .Append("Cancel")
                .ToArray();

            Menu scheduleMenu = new("Buy Tickets", menuItems);
            int choice = scheduleMenu.Run(); ;

            if (choice == upcomingSchedules.Count) return;

            Schedule selectedSchedule = upcomingSchedules[choice];
            Movie movie = movieService.GetMovieById(selectedSchedule.MovieId)!;

            if (movie.MinimumAge >= 18)
            {
                if (birthDates == null) birthDates = AgeVerifier.AskDateOfBirth(movie.MinimumAge.Value);

                if (!AgeVerifier.IsOldEnough(birthDates, movie.MinimumAge.Value))
                {
                    Console.WriteLine($"You must be at least {movie.MinimumAge} years old to watch {movie.Title}.");
                    Console.WriteLine("\nPress any key to choose another movie.");
                    Console.ReadKey();
                    continue;
                }
            }

            MovieRoom? movieRoom = movieRoomService.GetMovieRoomById(selectedSchedule.MovieRoomId);
            if (movieRoom is null) return;

            SeatSelection seatSelection = new();
            List<ScheduleSeat> selectedSeats = seatSelection.SelectSeatsMenu(Globals.SessionLocationId, movieRoom, selectedSchedule);

            if (selectedSeats.Count <= 0) return;

            List<ScheduleSeat> reservedSeats = scheduleSeatService.AddSeatToReservation(selectedSeats);

            OrderLineService orderLineService = new();
            List<OrderLine> orderLines = orderLineService.CreateOrderLineForCashDeskTickets(selectedSeats);

            CustomerOrderService customerOrderService = new();
            CustomerOrder customerOrder = customerOrderService.CreateCustomerOrderForCashDesk(orderLines);

            Receipt.DisplayReceipt(orderLines, customerOrder);

            string? input;
            do
            {
                Console.Write("\nConfirm payment (y) or (c) to cancel: ");
                input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y")
                {
                    CustomerOrder? customerOrderId = customerOrderService.RegisterCustomerOrder(customerOrder);

                    if (customerOrderId != null)
                    {
                        for (int i = 0; i < orderLines.Count; i++)
                        {
                            orderLines[i].CustomerOrderId = customerOrderId.Id;
                        }

                        orderLineService.ConnectCustomerOrderIdToOrderLine(orderLines);
                    }

                    string seatsString = string.Join("; ", reservedSeats.Select(s => s.SeatNumber));

                    Console.Clear();

                    Console.WriteLine($"Movie: {movie.Title}");
                    Console.WriteLine($"Date: {selectedSchedule.StartDate:dddd dd MMMM yyyy} {selectedSchedule.StartTime:hh\\:mm}");
                    Console.WriteLine($"Room: {movieRoom.RoomNumber}");
                    Console.WriteLine($"Seat(s): {seatsString}");

                    Console.WriteLine("\nPayment confirmed.");
                    break;
                }

                if (input == "c")
                {
                    scheduleSeatService.DeleteSeat(reservedSeats);
                    Console.WriteLine("\nPayment cancelled");
                    break;
                }

                else
                {
                    Console.WriteLine("Invalid input. Please enter 'y' to confirm or 'c' to cancel.");
                }
            } while (input != "y" && input != "c");

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}