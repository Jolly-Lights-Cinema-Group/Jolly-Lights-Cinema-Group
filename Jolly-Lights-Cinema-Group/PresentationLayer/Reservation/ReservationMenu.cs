namespace Jolly_Lights_Cinema_Group.Presentation
{
    public static class ReservationMenu
    {
        private static Menu _reservationMenu = new("Reservation Menu.", new string[] { "Make Reservation", "Edit Reservation", "Pay Reservation", "Back" });
        public static void ShowReservationMenu()
        {
            var inManageReservationsMenu = true;
            Console.Clear();

            while (inManageReservationsMenu)
            {
                int userChoice = _reservationMenu.Run();
                inManageReservationsMenu = HandleReservationChoice(userChoice);
                Console.Clear();
            }
        }

        private static bool HandleReservationChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    MakeReservationMenu makeReservationMenu = new();
                    makeReservationMenu.MakeReservation();
                    return true;
                case 1:
                    EditReservationMenu editReservationMenu = new();
                    editReservationMenu.EditReservation();
                    return true;
                case 2:
                    PayReservationMenu payReservationMenu = new();
                    payReservationMenu.PayReservation();
                    return true;
                case 3:
                    return false;
                default:
                    Console.WriteLine("Invalid selection.");
                    return false;
            }
        }
    }
}
