using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class CustomerOrderService
{
    private readonly CustomerOrderRepository _customerOrderRepository = new CustomerOrderRepository();
    public bool RegisterCustomerOrder(CustomerOrder customerOrder)
    {
        return _customerOrderRepository.AddCustomerOrder(customerOrder);
    }

    public CustomerOrder CreateCustomerOrderForReservation(Reservation reservation)
    {
        OrderLineRepository orderLineRepository = new();
        List<OrderLine> orderLines = orderLineRepository.GetOrderLinesByReservation(reservation);

        double totalPrice = 0;

        foreach (OrderLine orderLine in orderLines)
        {
            totalPrice += orderLine.Price;
        }

        CustomerOrder customerOrder = new(totalPrice);
        return customerOrder;
    }
}