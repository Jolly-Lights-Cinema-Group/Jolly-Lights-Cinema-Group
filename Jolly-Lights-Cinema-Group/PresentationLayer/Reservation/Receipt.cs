using Jolly_Lights_Cinema_Group;

public static class Receipt
{
    public static void DisplayReceipt(List<OrderLine> orderLines, CustomerOrder customerOrder)
    {
        foreach (OrderLine orderLine in orderLines)
        {
            Console.WriteLine($"{orderLine.Description} * {orderLine.Quantity} = €{Math.Round(orderLine.Price, 2)}     ({orderLine.VatPercentage}% VAT)");
        }
        Console.WriteLine($"-----------------------------------------------------------------------");
        Console.WriteLine($"Subtotal (excl. Tax): €{Math.Round(customerOrder.GrandPrice - customerOrder.Tax, 2)}");
        Console.WriteLine($"VAT: €{customerOrder.Tax}");
        Console.WriteLine($"Total (incl. Tax): €{customerOrder.GrandPrice}");
    }
}