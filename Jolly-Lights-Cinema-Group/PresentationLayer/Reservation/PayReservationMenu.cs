using Jolly_Lights_Cinema_Group;
using JollyLightsCinemaGroup.BusinessLogic;

public class PayReservationMenu
{
    private readonly ReservationService _reservationService;

    public PayReservationMenu()
    {
        _reservationService = new ReservationService();
    }
    public void PayReservation()
    {
        Console.Clear();

        string? reservationNumber;
        do
        {
            Console.Write("Enter reservation number: ");
            reservationNumber = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(reservationNumber));

        Reservation? reservation = _reservationService.FindReservationByReservationNumber(reservationNumber);

        if (reservation is null)
        {
            Console.WriteLine($"No reservation found with reservation number: {reservationNumber}");
        }
        else
        {
            CustomerOrderService customerOrderService = new();
            CustomerOrder customerOrder = customerOrderService.CreateCustomerOrderForReservation(reservation);

            if (!_reservationService.IsReservationPaid(reservation))
            {
                OrderLineService orderLineService = new();
                List<OrderLine> orderLines = orderLineService.GetOrderLinesByReservation(reservation);

                foreach (OrderLine orderLine in orderLines)
                {
                    Console.WriteLine($"{orderLine.Description} * {orderLine.Quantity} = €{orderLine.Price}     ({orderLine.VatPercentage}% VAT)");
                }
                Console.WriteLine($"-----------------------------------------------------------------------");
                Console.WriteLine($"Subtotal (excl. Tax): €{Math.Round(customerOrder.GrandPrice - customerOrder.Tax, 2)}");
                Console.WriteLine($"VAT: €{customerOrder.Tax}");
                Console.WriteLine($"Total (incl. Tax): €{customerOrder.GrandPrice}");

                string? input;
                do
                {
                    Console.Write("Confirm payment? (y/n): ");
                    input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "y")
                    {
                        if (_reservationService.PayReservation(reservation) && customerOrderService.RegisterCustomerOrder(customerOrder))
                        {
                            Console.WriteLine("Payment confirmed.");
                            break;
                        }
                        Console.WriteLine("Payment could not be confirmed.");
                        break;
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("Payment cancelled.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }

                } while (input != "y" && input != "n");
            }
            else Console.WriteLine($"Reservation: {reservation.ReservationNumber} has been paid");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }
}