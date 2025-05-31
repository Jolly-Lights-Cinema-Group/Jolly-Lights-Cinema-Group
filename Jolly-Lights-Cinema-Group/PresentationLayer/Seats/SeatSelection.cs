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

        var reservedSeats = reservationService.GetReservedSeats(movieRoom.Id!.Value);
        var roomLayout = PrepareRoomLayoutWithReservations(movieRoom, reservedSeats);

        do
        {
            StartSeatSelection(roomLayout);
        } while (!IsValidSelection(roomLayout, SelectedIndexY, SelectedIndexX, amount));

        Console.Clear();

        List<ScheduleSeat> selectedSeats = new List<ScheduleSeat>();

        for (int i = 0; i < SelectionAmount; i++)
        {
            var y = SelectedIndexY;
            var x = SelectedIndexX + i;
            var selectedSeatValue = roomLayout[y][x];

            var seatType = selectedSeatValue switch
            {
                "S" => SeatType.RegularSeat,
                "L" => SeatType.LoveSeat,
                "P" => SeatType.VipSeat,
                _ => throw new ArgumentOutOfRangeException()
            };

            var seatPrice = _seatService.GetSeatPriceForSeatTypeOnLocation(seatType, locationId);
            var seatCoord = $"{y},{x}";
            selectedSeats.Add(new ScheduleSeat(schedule.Id!.Value, seatPrice, seatType, seatCoord));
        }

        return selectedSeats;
    }

    private void StartSeatSelection(List<List<string>> seats)
    {
        MoveToNextValidPosition(seats);

        ConsoleKey keypressed;
        do
        {
            DisplayOptions(seats);

            keypressed = Console.ReadKey(true).Key;

            switch (keypressed)
            {
                case ConsoleKey.UpArrow:
                    do
                    {
                        SelectedIndexY--;
                    }
                    while (SelectedIndexY >= 0 &&
                        !IsValidSelection(seats, SelectedIndexY, SelectedIndexX, SelectionAmount));
                    if (SelectedIndexY < 0) SelectedIndexY = 0;
                    break;

                case ConsoleKey.DownArrow:
                    do
                    {
                        SelectedIndexY++;
                    }
                    while (SelectedIndexY < seats.Count &&
                        !IsValidSelection(seats, SelectedIndexY, SelectedIndexX, SelectionAmount));
                    if (SelectedIndexY >= seats.Count) SelectedIndexY = seats.Count - 1;
                    break;

                case ConsoleKey.LeftArrow:
                    do
                    {
                        SelectedIndexX--;
                    }
                    while (SelectedIndexX >= 0 &&
                        !IsValidSelection(seats, SelectedIndexY, SelectedIndexX, SelectionAmount));
                    if (SelectedIndexX < 0) SelectedIndexX = 0;
                    break;

                case ConsoleKey.RightArrow:
                    do
                    {
                        SelectedIndexX++;
                    }
                    while (SelectedIndexX < seats[SelectedIndexY].Count - SelectionAmount &&
                        !IsValidSelection(seats, SelectedIndexY, SelectedIndexX, SelectionAmount));
                    if (SelectedIndexX >= seats[SelectedIndexY].Count - SelectionAmount)
                        SelectedIndexX = seats[SelectedIndexY].Count - SelectionAmount;
                    break;
            }

            if (!IsValidSelection(seats, SelectedIndexY, SelectedIndexX, SelectionAmount))
                {
                    MoveToNextValidPosition(seats);
                }

        } while (keypressed != ConsoleKey.Enter);
        
    }

    private void DisplayOptions(List<List<string>> grid)
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

                bool isSelected = r == SelectedIndexY && c >= SelectedIndexX && c < SelectedIndexX + SelectionAmount;

                if (isSelected)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
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
    private bool IsValidSelection(List<List<string>> layout, int y, int x, int amount)
    {
        if (y < 0 || y >= layout.Count) return false;
        if (x < 0 || x + amount > layout[y].Count) return false;

        for (int i = 0; i < amount; i++)
        {
            string seat = layout[y][x + i];

            if (seat == "#" || seat == "_" || seat == "[X]")
                return false;
        }

        return true;
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

    private void MoveToNextValidPosition(List<List<string>> layout)
    {
        while (!IsValidSelection(layout, SelectedIndexY, SelectedIndexX, SelectionAmount))
        {
            SelectedIndexX++;
            if (SelectedIndexX >= layout[SelectedIndexY].Count)
            {
                SelectedIndexX = 0;
                SelectedIndexY++;
                if (SelectedIndexY >= layout.Count)
                {
                    SelectedIndexY = 0;
                }
            }
        }
    }
}
