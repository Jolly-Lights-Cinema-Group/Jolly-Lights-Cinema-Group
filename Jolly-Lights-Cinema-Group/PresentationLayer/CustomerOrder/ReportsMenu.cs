using Menu = Jolly_Lights_Cinema_Group.Menu;
public static class ReportsMenu
{
    private static CustomerOrderService _customerOrderService = new();
    private static Menu _manageMovieRoomnMenu = new("Reports Menu", new string[] { "Annual earnings", "Back" });
    public static void ShowReportsMenu()
    {
        bool inReportsMenu = true;
        Console.Clear();

        while (inReportsMenu)
        {
            int userChoice = _manageMovieRoomnMenu.Run();
            inReportsMenu = HandleReportsChoice(userChoice);
            Console.Clear();
        }
    }

    private static bool HandleReportsChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                AnnualEarnings();
                return true;
            case 1:
                return false;
            default:
                Console.WriteLine("Invalid selection.");
                return true;
        }
    }

    private static void AnnualEarnings()
    {
        Console.Clear();

        List<int> years = _customerOrderService.GetAvailableYears();

        if (years.Count == 0)
        {
            Console.WriteLine("No years available yet.");
            Console.ReadKey();
            return;
        }

        string[] yearOptions = years.Select(year => year.ToString()).Append("Cancel").ToArray();
        Menu yearMenu = new("Select a year:", yearOptions);
        int selectedIndex = yearMenu.Run();

        if (selectedIndex == yearOptions.Length - 1)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        int selectedYear = years[selectedIndex];

        double grossIncome = _customerOrderService.GetGrossAnualEarnings(selectedYear);
        double netIncome = _customerOrderService.GetNetAnualEarnings(selectedYear);

        Console.Clear();

        Console.WriteLine($"Year: {selectedYear}");
        Console.WriteLine($"----------------------------------");
        Console.WriteLine($"Gross: €{grossIncome}");
        Console.WriteLine($"Tax: €{Math.Round(grossIncome - netIncome, 2)}");
        Console.WriteLine($"----------------------------------");
        Console.WriteLine($"Net: €{netIncome}");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }
}
