using Dollet.Core.Entities;

namespace Dollet.Core.Abstractions.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAllAsync();
        void Update(Currency currency);
        Task<Currency?> GetDefaultAsync();
        Task<Dictionary<string, decimal>> GetCurrencyValuesAsync(string code);
        Task<bool> AnyAsync();
        void AddMany(IEnumerable<Currency> currencies);
        void AddManyValues(IEnumerable<CurrencyValue> currencyValues);
        Task DeleteValuesAsync();
    }
}
