using Xunit;
using Moq;
using JollyLightsCinemaGroup.DataAccess;

public class CustomerOrderServiceTest
{
    private readonly Mock<CustomerOrderRepository> _mockCustomerOrderRepository;
    private readonly Mock<OrderLineRepository> _mockOrderLineRepository;
    private readonly CustomerOrderService _customerOrderService;

    public CustomerOrderServiceTest()
    {
        _mockCustomerOrderRepository = new Mock<CustomerOrderRepository>();
        _mockOrderLineRepository = new Mock<OrderLineRepository>();
        _customerOrderService = new CustomerOrderService(_mockCustomerOrderRepository.Object, _mockOrderLineRepository.Object);
    }

    [Fact]
    public void TestCreateCustomerOrderForReservation()
    {
        Reservation reservation = new Reservation(1, "John", "Doe", 068112347, "john.doe@gmail.com", "x", false);
        List<OrderLine> orderLines = new List<OrderLine>
        {
            new OrderLine(5, "Popcorn S", 9, 10),
            new OrderLine(2, "Cola", 9, 6),
            new OrderLine(1, "Heineken", 21, 4)
        };

        _mockOrderLineRepository
            .Setup(repo => repo.GetOrderLinesByReservation(reservation))
            .Returns(orderLines);

        CustomerOrder result = _customerOrderService.CreateCustomerOrderForReservation(reservation);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(22.28, result.GrandPrice);
        Xunit.Assert.Equal(2.28, result.Tax);
    }

    [Fact]
    public void TestCreateCustomerOrderForCashDesk()
    {
        List<OrderLine> orderLines = new List<OrderLine>
        {
            new OrderLine(10, "Popcorn S", 9, 20),
            new OrderLine(5, "Cola", 9, 15),
            new OrderLine(3, "Heineken", 21, 12)
        };

        CustomerOrder result = _customerOrderService.CreateCustomerOrderForCashDesk(orderLines);

        // 21.80 + 16.35 + 14.42
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(52.67, result.GrandPrice);
        Xunit.Assert.Equal(5.67, result.Tax);
    }

    [Fact]
    public void TestGetGrossAnualEarnings()
    {
        int testYear = 2025;
        List<CustomerOrder> orders = new List<CustomerOrder>
        {
            new CustomerOrder(100.0, System.DateTime.Now, 10.0),
            new CustomerOrder(200.0, System.DateTime.Now, 20.0),
            new CustomerOrder(50.0, System.DateTime.Now, 5.0),
        };
        _mockCustomerOrderRepository.Setup(repo => repo.GetCustomerOrdersByYear(testYear)).Returns(orders);

        double result = _customerOrderService.GetGrossAnualEarnings(testYear);

        // 100 + 200 + 50
        Xunit.Assert.Equal(350.00, result);
    }

    [Fact]
    public void TestGetNetAnualEarnings()
    {
        int testYear = 2025;
        List<CustomerOrder> orders = new List<CustomerOrder>
        {
            new CustomerOrder(100.0, System.DateTime.Now, 10.0),
            new CustomerOrder(200.0, System.DateTime.Now, 20.0),
            new CustomerOrder(50.0, System.DateTime.Now, 5.0),
        };
        _mockCustomerOrderRepository.Setup(repo => repo.GetCustomerOrdersByYear(testYear)).Returns(orders);

        double result = _customerOrderService.GetNetAnualEarnings(testYear);

        // (100 + 200 + 50) - 35
        Xunit.Assert.Equal(315.00, result);
    }
}