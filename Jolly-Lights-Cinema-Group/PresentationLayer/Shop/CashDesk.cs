using Jolly_Lights_Cinema_Group;
using Jolly_Lights_Cinema_Group.Common;

public static class CashDesk
{
    private static ShopItemService _shopItemService = new();
    private static Menu _cashDesk = new("Shop Management", new string[] { "Tickets", "Shop", "Back" });
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
                ShopMenu shopMenu = new();
                shopMenu.CashDeskShop(Globals.SessionLocationId);
                return true;
            case 1:
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

    }
}