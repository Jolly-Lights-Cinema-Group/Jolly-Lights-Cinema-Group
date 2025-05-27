using Jolly_Lights_Cinema_Group.Common;
using Jolly_Lights_Cinema_Group.Enum;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ReservationHandler
    {
        private static int SelectedIndexY;
        private static int SelectedIndexX;

        public static void AddReservation()
        {
            int locationId;
            if (Globals.CurrentUser!.Role == Role.Admin)
            {

                LocationMenu location = new();
                int selectedLocation = location.Run();

                LocationService locationService = new LocationService();
                List<Location> locations = locationService.GetAllLocations();

                locationId = (int)locations[selectedLocation].Id!;
            }
            else locationId = Globals.SessionLocationId;

            MovieScheduleMenu movieScheduleMenu = new();
            Movie? selectedMovie = movieScheduleMenu.SelectMovieMenu(locationId);

            if (selectedMovie is null) return;

            MovieDateTimeMenu movieDateTimeMenu = new();
            Schedule? selectedSchedule = movieDateTimeMenu.SelectSchedule(selectedMovie, locationId);

            if (selectedSchedule is null) return;

            MovieRoomRepository movieRoomRepository = new();
            MovieRoom? movieRoom = movieRoomRepository.GetMovieRoomById(selectedSchedule.MovieRoomId);

            if (movieRoom is null) return;

            var roomId = selectedSchedule.MovieRoomId;
            var scheduleId = selectedSchedule.Id;
            
            MovieRoomService movieRoomService = new MovieRoomService();
            ReservationService reservationService = new ReservationService();
            
            var roomLayout = movieRoomService.GetRoomLayout(movieRoom.Id!.Value);
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