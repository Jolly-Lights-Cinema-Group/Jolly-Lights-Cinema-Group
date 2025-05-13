using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class CustomerOrderService
{
    private readonly CustomerOrderRepository _customerOrderRepository = new CustomerOrderRepository();
    public void RegisterCustomerOrder(CustomerOrder customerOrder)
    {
        _customerOrderRepository.AddCustomerOrder(customerOrder);
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
        RegisterCustomerOrder(customerOrder);

        ReservationService reservationService = new();
        reservationService.PayReservation(reservation);

        return customerOrder;
    }
}
