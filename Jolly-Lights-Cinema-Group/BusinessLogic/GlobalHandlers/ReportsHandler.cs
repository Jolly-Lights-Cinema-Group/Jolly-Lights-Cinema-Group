using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ReportsHandler
    {
        public static void ManageReports()
        {
            bool inManageReportsMenu = true;
            ReportsMenu reportsMenu = new();
            Console.Clear();

            while (inManageReportsMenu)
            {
                int userChoice = reportsMenu.Run();
                inManageReportsMenu = HandleManageReportsChoice(userChoice);
                Console.Clear();
            }
        }
        private static bool HandleManageReportsChoice(int choice)
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

            CustomerOrderRepository customerOrderRepository = new();
            List<int> years = customerOrderRepository.GetAvailableYears();

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

            CustomerOrderService customerOrderService = new();
            double totalIncome = customerOrderService.GetAnualEarnings(selectedYear);
    
            string[] grossNetOptions = { "Gross", "Net", "Cancel" };
            Menu grossNetMenu = new("Select calculation type:", grossNetOptions);
            int grossNetChoice = grossNetMenu.Run();

            if (grossNetChoice == 2)
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            bool isGross = grossNetChoice == 0;
            double calculatedTotal = isGross ? totalIncome : totalIncome * 0.91;

            Console.WriteLine($"{(isGross ? "Gross" : "Net")} total for {selectedYear}: €{calculatedTotal}");

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }

}