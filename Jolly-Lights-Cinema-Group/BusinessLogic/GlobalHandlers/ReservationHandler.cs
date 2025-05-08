using JollyLightsCinemaGroup.BusinessLogic;
using JollyLightsCinemaGroup.DataAccess;

namespace Jolly_Lights_Cinema_Group
{
    public static class ReservationHandler
    {
        public static void ManageReservations()
        {
            bool inManageReservationsMenu = true;
            ReservationMenu reservationMenu = new();
            Console.Clear();
            
            while(inManageReservationsMenu)
            {
            int userChoice = reservationMenu.Run();
            inManageReservationsMenu = HandleManageReservationsChoice(userChoice);
            Console.Clear();
            }
        }
        private static bool HandleManageReservationsChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    AddReservation();
                    return true;
                case 1:
                    DeleteReservation();
                    return true;
                case 2:
                    GetReservation();
                    return true;
                case 3:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return true;
            }
        }
        public static void AddReservation()
        {
            Console.Clear();

            Console.WriteLine("Add Reservation:");
            string? firstName;
            do
            {
                Console.Write("Enter first name: ");
                firstName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(firstName));

            string? lastName;
            do
            {
                Console.Write("Enter last name: ");
                lastName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(lastName));

            int phoneNumber;
            string? input;
            do
            {
                Console.Write("Enter telephone number: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out phoneNumber) || phoneNumber < 0);

            string? eMail;
            do
            {
                Console.Write("Enter email address: ");
                eMail = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(eMail));

            ReservationService reservationService = new ReservationService();

            string reservationNumber = ReservationNumberGenerator.GetReservationNumber();

            Reservation reservation = new(firstName, lastName, phoneNumber, eMail, reservationNumber);

            reservationService.RegisterReservation(reservation);

            ReservationRepository reservationRepository = new();
            Reservation newReservation = reservationRepository.FindReservationByReservationNumber(reservation.ReservationNumber)!;

            Console.Write("\nWould you like to add extra items to your reservation? (y/n): ");
            string? response = Console.ReadLine()?.Trim().ToLower();

            if (response == "y")
            {
                ShopHandler.ManageShop(newReservation);
            }

            OrderLineService orderLineService = new();
            orderLineService.CreateOrderLineForReservation(newReservation);

            Console.WriteLine("\nReservation complete. Press any key to continue.");
            Console.ReadKey();
        }


        public static void DeleteReservation()
        {
            Console.Clear();

            Console.WriteLine("Delete reservation:");

            string? reservationNumber;
            do
            {
                Console.Write("Enter reservation number: ");
                reservationNumber = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(reservationNumber));


            ReservationRepository reservationRepository = new ReservationRepository();
            Reservation? reservation = reservationRepository.FindReservationByReservationNumber(reservationNumber);
            if (reservation != null)
            {
                ReservationService reservationService = new ReservationService();
                reservationService.DeleteReservation(reservation);
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        public static void GetReservation()
        {
            Console.Clear();

            Console.WriteLine("Find reservation:");

            string? reservationNumber;
            do
            {
                Console.Write("Enter reservation number: ");
                reservationNumber = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(reservationNumber));


            ReservationService reservationService = new ReservationService();
            reservationService.FindReservationByReservationNumber(reservationNumber);

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
    }
}