namespace ConsoleApp.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static bool IsBusinessDay(this DateTime dateTime) => dateTime.DayOfWeek switch
        {
            DayOfWeek.Monday => true,
            DayOfWeek.Tuesday => true,
            DayOfWeek.Wednesday => true,
            DayOfWeek.Thursday => true,
            DayOfWeek.Friday => true,
            DayOfWeek.Saturday => false,
            DayOfWeek.Sunday => false,
            _ => throw new NotImplementedException(),
        };
    }
}
