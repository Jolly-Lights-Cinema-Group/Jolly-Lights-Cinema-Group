using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class OrderLineService
{
    private readonly OrderLineRepository _orderLineRepo;

    public OrderLineService()
    {
        _orderLineRepo = new OrderLineRepository();
    }
    public void RegisterOrderLine(int orderId, int quantity, string description, int vatPercentage, double price)
    {
        _orderLineRepo.AddOrderLine(orderId, quantity, description, vatPercentage, price);
    }
}
