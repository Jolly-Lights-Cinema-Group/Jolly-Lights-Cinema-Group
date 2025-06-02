using Jolly_Lights_Cinema_Group.BusinessLogic;
using Jolly_Lights_Cinema_Group.Enum;

public class SeatSelection
{
    private readonly SeatService _seatService;
    private int SelectedIndexY = 0;
    private int SelectedIndexX = 0;
    private int SelectionAmount = 1;

    public SeatSelection()
    {
        _seatService = new SeatService();
    }

    public List<ScheduleSeat> SelectSeatsMenu(int locationId, MovieRoom movieRoom, Schedule schedule, int amount)
    {
        ReservationService reservationService = new ReservationService();
        SelectionAmount = amount;

        var reservedSeats = reservationService.GetReservedSeats(schedule);
        var roomLayout = PrepareRoomLayoutWithReservations(movieRoom, reservedSeats);

        var selectedSeatsCoords = new List<(int y, int x)>();

        while (selectedSeatsCoords.Count < SelectionAmount)
        {
            StartSeatSelection(roomLayout, selectedSeatsCoords);
        }

        Console.Clear();

        List<ScheduleSeat> selectedSeats = new List<ScheduleSeat>();

        foreach (var (y, x) in selectedSeatsCoords)
        {
            var seatValue = roomLayout[y][x];
            var seatType = seatValue switch
            {
                "S" => SeatType.RegularSeat,
                "L" => SeatType.LoveSeat,
                "P" => SeatType.VipSeat,
                _ => throw new ArgumentOutOfRangeException()
            };

            var seatPrice = _seatService.GetSeatPriceForSeatTypeOnLocation(seatType, locationId);
            selectedSeats.Add(new ScheduleSeat(schedule.Id!.Value, seatPrice, seatType, $"{y},{x}"));
        }

        return selectedSeats;
    }

    private void StartSeatSelection(List<List<string>> seats, List<(int y, int x)> alreadySelected)
    {
        MoveToNextValidPosition(seats, alreadySelected);

        ConsoleKey keyPressed;
        do
        {
            DisplayOptions(seats, alreadySelected);

            keyPressed = Console.ReadKey(true).Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    (SelectedIndexY, SelectedIndexX) = MoveCursor(seats, SelectedIndexY, SelectedIndexX, -1, 0, alreadySelected);
                    break;

                case ConsoleKey.DownArrow:
                    (SelectedIndexY, SelectedIndexX) = MoveCursor(seats, SelectedIndexY, SelectedIndexX, 1, 0, alreadySelected);
                    break;

                case ConsoleKey.LeftArrow:
                    (SelectedIndexY, SelectedIndexX) = MoveCursor(seats, SelectedIndexY, SelectedIndexX, 0, -1, alreadySelected);
                    break;

                case ConsoleKey.RightArrow:
                    (SelectedIndexY, SelectedIndexX) = MoveCursor(seats, SelectedIndexY, SelectedIndexX, 0, 1, alreadySelected);
                    break;
            }
        } while (keyPressed != ConsoleKey.Enter ||
                 !IsSelectablePosition(seats, SelectedIndexY, SelectedIndexX, alreadySelected));

        alreadySelected.Add((SelectedIndexY, SelectedIndexX));
    }

    private void DisplayOptions(List<List<string>> grid, List<(int y, int x)> alreadySelected)
    {
        Console.Clear();
        for (var r = 0; r < grid.Count; r++)
        {
            for (var c = 0; c < grid[r].Count; c++)
            {
                string displayValue = grid[r][c];

                displayValue = displayValue switch
                {
                    "S" => "[_]",
                    "L" => "[L]",
                    "P" => "[V]",
                    "[X]" => "[X]",
                    "_" or "#" => "   ",
                    _ => $"[{displayValue}]"
                };

                bool isCurrent = r == SelectedIndexY && c == SelectedIndexX;
                bool isChosen = alreadySelected.Contains((r, c));


                if (isCurrent)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("[*]");
                    Console.ResetColor();
                }
                else if (isChosen)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("[*]");
                    Console.ResetColor();
                }
                else if (displayValue == "[X]")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(displayValue);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(displayValue);
                }
            }
            Console.WriteLine();
        }
    }

    private List<List<string>> PrepareRoomLayoutWithReservations(MovieRoom movieRoom, List<(string, string)> reservedSeats)
    {
        MovieRoomService movieRoomService = new MovieRoomService();
        var roomLayout = movieRoomService.GetRoomLayout(movieRoom.Id!.Value);

        for (int y = 0; y < roomLayout!.Count; y++)
        {
            for (int x = 0; x < roomLayout[y].Count; x++)
            {
                string seatCoord = $"{y},{x}";
                if (reservedSeats.Any(s => $"{s.Item1},{s.Item2}" == seatCoord))
                {
                    roomLayout[y][x] = "[X]";
                }
            }
        }

        return roomLayout;
    }

    private void MoveToNextValidPosition(List<List<string>> layout, List<(int y, int x)> alreadySelected)
    {
        int totalRows = layout.Count;
        int totalCols = layout[0].Count;

        while (!IsSelectablePosition(layout, SelectedIndexY, SelectedIndexX, alreadySelected))
        {
            SelectedIndexX++;
            if (SelectedIndexX >= totalCols)
            {
                SelectedIndexX = 0;
                SelectedIndexY++;
                if (SelectedIndexY >= totalRows)
                {
                    SelectedIndexY = 0;
                }
            }
        }
    }

    private bool IsSelectablePosition(List<List<string>> layout, int y, int x, List<(int y, int x)> alreadySelected)
    {
        if (y < 0 || y >= layout.Count) return false;
        if (x < 0 || x >= layout[y].Count) return false;

        string seat = layout[y][x];

        if (seat == "#" || seat == "_" || seat == "[X]") return false;
        if (alreadySelected.Contains((y, x))) return false;

        return true;
    }

    private (int newY, int newX) MoveCursor(List<List<string>> layout, int startY, int startX, int deltaY, int deltaX, List<(int y, int x)> alreadySelected)
    {
        int maxRows = layout.Count;
        int maxCols = layout[0].Count;

        int y = startY;
        int x = startX;

        while (true)
        {
            y += deltaY;
            x += deltaX;

            if (y < 0 || y >= maxRows || x < 0 || x >= maxCols)
                break;

            if (IsSelectablePosition(layout, y, x, alreadySelected))
                return (y, x);
        }

        if (deltaY != 0)
        {
            for (int row = startY + deltaY; row >= 0 && row < maxRows; row += deltaY)
            {
                for (int col = 0; col < maxCols; col++)
                {
                    if (IsSelectablePosition(layout, row, col, alreadySelected))
                        return (row, col);
                }
            }
        }

        else if (deltaX != 0)
        {
            for (int col = startX + deltaX; col >= 0 && col < maxCols; col += deltaX)
            {
                for (int row = 0; row < maxRows; row++)
                {
                    if (IsSelectablePosition(layout, row, col, alreadySelected))
                        return (row, col);
                }
            }
        }

        return (startY, startX);
    }
}
