namespace ClinicProject1.Extensions
{
    public static class TimeFormatter
    {
        public static string Format(TimeSpan time)
        {
            return time.ToString(@"hh\:mm");
        }

        public static TimeSpan Parse(string timeString)
        {
            if (TimeSpan.TryParseExact(timeString, @"hh\:mm", null, out var time))
            {
                return time;
            }
            throw new FormatException("Invalid time format. Expected HH:mm");
        }
    }
}
