namespace Dollet.Core.Abstractions.Services
{
    public interface IDateTimeRangeService
    {
        DateTime GetToday();
        (DateTime, DateTime) GetWeekly();
        (DateTime, DateTime) GetMonthly();
    }
}
