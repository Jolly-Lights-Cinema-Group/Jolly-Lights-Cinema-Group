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

}
