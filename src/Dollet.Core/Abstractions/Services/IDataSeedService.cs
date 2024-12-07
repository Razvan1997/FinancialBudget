namespace Dollet.Core.Abstractions.Services
{
    public interface IDataSeedService
    {
        Task SeedCategoriesAsync();
        Task SeedCurrenciesAsync();
        Task SeedCurrencyValuesAsync(string currency = "PLN");
        Task SeedUsersAsync();
    }
}
