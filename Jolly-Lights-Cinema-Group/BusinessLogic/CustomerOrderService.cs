using JollyLightsCinemaGroup.DataAccess;

public class CustomerOrderService
{
    private readonly CustomerOrderRepository _customerOrderRepository = new CustomerOrderRepository();
    public CustomerOrder? RegisterCustomerOrder(CustomerOrder customerOrder)
    {
        return _customerOrderRepository.AddCustomerOrder(customerOrder);
    }

    public CustomerOrder CreateCustomerOrderForReservation(Reservation reservation)
    {
        OrderLineRepository orderLineRepository = new();
        List<OrderLine> orderLines = orderLineRepository.GetOrderLinesByReservation(reservation);

        double grandTotal = 0;
        double tax = 0;

        foreach (OrderLine orderLine in orderLines)
        {
            double vat = orderLine.VatPercentage / 100.0;
            double taxPerOrderLine = orderLine.Price * vat;
            tax += taxPerOrderLine;
            grandTotal += orderLine.Price + taxPerOrderLine;
        }

        CustomerOrder customerOrder = new(Math.Round(grandTotal, 2), DateTime.Now, Math.Round(tax, 2));
        return customerOrder;
    }

    public CustomerOrder CreateCustomerOrderForCashDesk(List<OrderLine> orderLines)
    {
        double grandTotal = 0;
        double tax = 0;

        foreach (OrderLine orderLine in orderLines)
        {
            double vat = orderLine.VatPercentage / 100.0;
            double taxPerOrderLine = orderLine.Price * vat;
            tax += taxPerOrderLine;
            grandTotal += orderLine.Price + taxPerOrderLine;
        }

        CustomerOrder customerOrder = new(Math.Round(grandTotal, 2), DateTime.Now, Math.Round(tax, 2));
        return customerOrder;
    }

    public double GetGrossAnualEarnings(int year)
    {
        double total = 0;
        List<CustomerOrder> customerOrders = _customerOrderRepository.GetCustomerOrdersByYear(year);

        foreach (CustomerOrder customerOrder in customerOrders)
        {
            total += customerOrder.GrandPrice;
        }
        return Math.Round(total, 2);
    }

    public double GetNetAnualEarnings(int year)
    {
        double total = 0;
        List<CustomerOrder> customerOrders = _customerOrderRepository.GetCustomerOrdersByYear(year);

        foreach (CustomerOrder customerOrder in customerOrders)
        {
            total += customerOrder.GrandPrice - customerOrder.Tax;
        }
        return Math.Round(total, 2);
    }

    public List<int> GetAvailableYears()
    {
        return _customerOrderRepository.GetAvailableYears();
    }
}
