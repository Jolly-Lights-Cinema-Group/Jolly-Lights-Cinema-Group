using JollyLightsCinemaGroup.DataAccess;

public class CustomerOrderService
{
    private readonly CustomerOrderRepository _customerOrderRepository;
    private readonly OrderLineRepository _orderLineRepository;

    public CustomerOrderService(CustomerOrderRepository? customerOrderRepository = null, OrderLineRepository? orderLineRepository = null)
    {
        _customerOrderRepository = customerOrderRepository ?? new CustomerOrderRepository();
        _orderLineRepository = orderLineRepository ?? new OrderLineRepository();
    }

    public CustomerOrder? RegisterCustomerOrder(CustomerOrder customerOrder)
    {
        return _customerOrderRepository.AddCustomerOrder(customerOrder);
    }

    public CustomerOrder CreateCustomerOrderForReservation(Reservation reservation)
    {
        List<OrderLine> orderLines = _orderLineRepository.GetOrderLinesByReservation(reservation);

        return CreateCustomerOrderForCashDesk(orderLines);
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

    public decimal GetEarningsForYearMonth(int year, int month)
    {
        return _customerOrderRepository.GetEarningsForYearMonth(year, month);
    }

    public decimal GetNetEarningsForYearMonth(int year, int month)
    {
        return _customerOrderRepository.GetNetEarningsForYearMonth(year, month);
    }
}
