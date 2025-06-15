using Jolly_Lights_Cinema_Group.Helpers;

public static class AgeVerifier
{
    public static bool IsOldEnough(DateTime birthDate, int minimumAge)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
            age--;

        return age >= minimumAge;
    }

    public static DateTime AskDateOfBirth()
    {
        string? dateOfBirthInput;
        DateTime dateOfBirth;
        do
        {
            Console.Write("Enter date of birth (dd/MM/yyyy): ");
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
        return dateOfBirth;
    }
}
