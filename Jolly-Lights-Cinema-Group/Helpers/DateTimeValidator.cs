using System.Globalization;

namespace Jolly_Lights_Cinema_Group.Helpers
{
    public static class DateTimeValidator
    {
        public static bool TryParseDate(string? input, out DateTime result)
        {
            return DateTime.TryParseExact(input, "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result);
        }

        public static bool TryParseTime(string? input, out TimeSpan result)
        {
            return TimeSpan.TryParseExact(input, "hh\\:mm\\:ss",
                CultureInfo.InvariantCulture,
                out result);
        }
    }
}
