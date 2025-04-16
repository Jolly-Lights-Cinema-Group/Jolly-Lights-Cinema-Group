using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public static class CustomerOrderService
{
    public static void RegisterCustomerOrder(CustomerOrder customerOrder)
    {
        CustomerOrderRepository.AddCustomerOrder(customerOrder);
    }

}
