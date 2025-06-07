using System.Globalization;
using Menu = Jolly_Lights_Cinema_Group.Menu;
public static class ReportsMenu
{
    private static CustomerOrderService _customerOrderService = new();
    private static Menu _manageMovieRoomnMenu = new("Reports Menu", new string[] { "Annual earnings", "Monthly earnings", "Back" });
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
                MonthlyEarnings();
                return true;
            case 2:
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

    private static void MonthlyEarnings()
    {
        Console.Clear();
        // Year Menu
        List<int> years = _customerOrderService.GetAvailableYears();
        if (years.Count == 0)
        {
            Console.WriteLine("No years available yet.");
            Console.ReadKey();
            return;
        }

        string[] yearOptions = years.Select(y => y.ToString())
                                    .Append("Cancel")
                                    .ToArray();

        Menu yearMenu = new("Select a year:", yearOptions);
        int selectedYearIndex = yearMenu.Run();

        if (selectedYearIndex == yearOptions.Length - 1)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        int selectedYear = years[selectedYearIndex];

        // Month Menu
        string[] monthOptions = Enumerable.Range(1, 12)
        .Select(m => $"{new DateTime(2000, m, 1).ToString("MMMM", CultureInfo.InvariantCulture)}")
        .Append("Cancel")
        .ToArray();

        Menu monthMenu = new("Select a month:", monthOptions);
        int selectedMonthIndex = monthMenu.Run();

        if (selectedMonthIndex == monthOptions.Length - 1)
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        int selectedMonth = selectedMonthIndex + 1;

        decimal grossIncome = _customerOrderService.GetEarningsForYearMonth(selectedYear, selectedMonth);
        decimal netIncome = _customerOrderService.GetNetEarningsForYearMonth(selectedYear, selectedMonth);

        Console.Clear();


        Console.WriteLine($"Year: {selectedYear}\n");
        Console.WriteLine($"Month: {new DateTime(2000, selectedMonth, 1).ToString("MMMM", CultureInfo.InvariantCulture)}");
        Console.WriteLine($"----------------------------------");
        Console.WriteLine($"Gross: €{grossIncome}");
        Console.WriteLine($"Tax: €{Math.Round(grossIncome - netIncome, 2)}");
        Console.WriteLine($"----------------------------------");
        Console.WriteLine($"Net: €{netIncome}");

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }
}
