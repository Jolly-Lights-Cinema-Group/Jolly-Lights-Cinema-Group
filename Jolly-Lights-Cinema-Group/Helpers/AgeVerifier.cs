using Jolly_Lights_Cinema_Group.Helpers;

public static class AgeVerifier
{
    public static bool IsOldEnough(List<DateTime> birthdates, int minimumAge)
    {
        DateTime today = DateTime.Today;

        foreach (DateTime birthDate in birthdates)
        {
            int age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            if (age < minimumAge) return false;
        }
        return true;
    }

    public static List<DateTime> AskDateOfBirth(int minimumAge)
    {
        List<DateTime> birthDates = new();
        bool tryAnother = true;

        while (tryAnother)
        {
            string? dateOfBirthInput;
            DateTime dateOfBirth;
            do
            {
                Console.Write("\nEnter date of birth (dd/MM/yyyy): ");
                dateOfBirthInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(dateOfBirthInput))
                {
                    Console.WriteLine("Date of birth cannot be empty. Please try again.");
                    continue;
                }

                if (!DateTimeValidator.TryParseDate(dateOfBirthInput, out dateOfBirth))
                {
                    Console.WriteLine("Invalid date format. Please enter a valid date (e.g. 22/02/2025).");
                    continue;
                }

                if (dateOfBirth > DateTime.Now)
                {
                    Console.WriteLine("Date of birth cannot be in the future. Please enter a valid date.");
                    continue;
                }
                break;
            } while (true);

            birthDates.Add(dateOfBirth);

            if (!IsOldEnough(birthDates, minimumAge))
            {
                break;
            }

            string? response;
            do
            {
                Console.Write("Verify another person? (y/n): ");
                response = Console.ReadLine()?.Trim().ToLower();

                if (response == "n")
                {
                    tryAnother = false;
                    break;
                }

                if (response == "y") break;

                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
            } while (true);
        }

        return birthDates;
    }
}
