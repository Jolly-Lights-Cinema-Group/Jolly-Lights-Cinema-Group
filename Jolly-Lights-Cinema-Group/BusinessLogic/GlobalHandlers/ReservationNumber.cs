using System.Text;

using JollyLightsCinemaGroup.DataAccess;

namespace JollyLightsCinemaGroup.BusinessLogic
{
    public static class ReservationNumberGenerator
    {
        private static readonly ReservationRepository _reservationRepository = new ReservationRepository();
        private static readonly char[] AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ1234567890".ToCharArray();

        private static readonly Random random = new();
        public static string GetReservationNumber()
        {
            var existingReservationNumbers = _reservationRepository.GetAllReservations().Select(r => r.ReservationNumber).ToHashSet();

            string reservationNumber;
            do
            {
                reservationNumber = GenerateReservationNumber(10);
            }
            while (existingReservationNumbers.Contains(reservationNumber));

            return reservationNumber;
        }

        private static string GenerateReservationNumber(int length)
        {
            StringBuilder reservationNumber = new();

            for (int i = 0; i < length; i++)
            {
                char character = AllowedChars[random.Next(AllowedChars.Length)];
                reservationNumber.Append(character);
            }

            return reservationNumber.ToString();
        }
    }
}
