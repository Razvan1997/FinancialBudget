using Dollet.Core.Abstractions.Services;

namespace Dollet.Infrastructure.Services
{
    internal class DateTimeRangeService : IDateTimeRangeService
    {
        public DateTime GetToday()
        {
            return DateTime.Today;
        }

        public (DateTime, DateTime) GetWeekly()
        {
            var today = DateTime.Today;

            int daysUntilMonday = DayOfWeek.Monday - today.DayOfWeek;
            var monday = today.AddDays(daysUntilMonday);

            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                monday = today.AddDays(-6);
            }

            var sunday = monday.AddDays(6);
            return (monday, sunday);
        }

        public (DateTime, DateTime) GetMonthly()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            return (firstDayOfMonth, lastDayOfMonth);
        }
    }
}
