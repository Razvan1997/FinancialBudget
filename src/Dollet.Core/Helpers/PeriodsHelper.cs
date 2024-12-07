using Dollet.Core.Abstractions.Services;
using Dollet.Core.Enums;
namespace Dollet.Core.Helpers
{
    public class PeriodsHelper(IDateTimeRangeService dateTimeRangeService)
    {
        private readonly IDateTimeRangeService _dateOnlyRangeService = dateTimeRangeService;

        public (DateTime, DateTime) GetDateRangeBasedOnSelectedPeriod(string selectedPeriod)
        {
            switch (selectedPeriod)
            {
                case nameof(TransactionsPeriod.Daily):
                    var today = _dateOnlyRangeService.GetToday();
                    return (today, today);

                case nameof(TransactionsPeriod.Weekly):
                    return _dateOnlyRangeService.GetWeekly();

                case nameof(TransactionsPeriod.Monthly):
                    return _dateOnlyRangeService.GetMonthly();

                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedPeriod), $"Unknown date range: {selectedPeriod}.");
            }
        }

        public static string GetStringPeriod((DateTime, DateTime) dateRange)
        {
            if (dateRange.Item1 == dateRange.Item2)
            {
                return dateRange.Item1.ToString("dd.MM.yyyy");
            }
            return $"{dateRange.Item1:dd.MM.yyyy} - {dateRange.Item2:dd.MM.yyyy}";
        }
    }
}
