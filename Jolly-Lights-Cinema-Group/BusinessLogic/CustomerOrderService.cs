using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class CustomerOrderService
{
    private readonly CustomerOrderRepository _customerOrderRepo;

    public CustomerOrderService()
    {
        _customerOrderRepo = new CustomerOrderRepository();
    }
    public void RegisterCustomerOrder(double grandPrice)
    {
        if (double.IsNegative(grandPrice))
        {
            Console.WriteLine("Error: Grand price can not be negative.");
            return;
        }

        _customerOrderRepo.AddCustomerOrder(grandPrice);
    }

    public void ShowAllCustomerOrders()
    {
        List<string> customerOrders = _customerOrderRepo.GetAllCustomerOrders();
        if (customerOrders.Count == 0)
        {
            Console.WriteLine("No customer orders found.");
        }
        else
        {
            Console.WriteLine("Customer Orders:");
            foreach (var order in customerOrders)
            {
                Console.WriteLine(order);
            }
        }
    }
}
