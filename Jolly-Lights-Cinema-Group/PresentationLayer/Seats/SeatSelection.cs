using Jolly_Lights_Cinema_Group.BusinessLogic;
using Jolly_Lights_Cinema_Group.Enum;

public class SeatSelection
{
    private readonly SeatService _seatService;
    private static int SelectedIndexY;
    private static int SelectedIndexX;

    public SeatSelection()
    {
        _seatService = new SeatService();
    }

    public ScheduleSeat SelectSeatsMenu(int locationId, MovieRoom movieRoom, Schedule schedule)
    {
        MovieRoomService movieRoomService = new MovieRoomService();
        ReservationService reservationService = new ReservationService();

        var roomLayout = movieRoomService.GetRoomLayout(movieRoom.Id!.Value);
        var reservedSeats = reservationService.GetReservedSeats(movieRoom.Id!.Value);

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
        } while (!IsValidSelection(roomLayout, SelectedIndexY, SelectedIndexX));

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

        var seatPrice = _seatService.GetSeatPriceForSeatTypeOnLocation(seatType, locationId);

        ScheduleSeat scheduleSeat = new(schedule.Id!.Value, seatPrice, seatType, selectedSeat);
        return scheduleSeat;
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
    private static bool IsValidSelection(List<List<string>> layout, int y, int x)
    {
        if (y < 0 || y >= layout.Count) return false;
        if (x < 0 || x >= layout[y].Count) return false;

        string seat = layout[y][x];
        return seat != "#" && seat != "_";
    }
}
