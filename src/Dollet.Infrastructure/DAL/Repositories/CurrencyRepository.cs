using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL.Repositories
{
    internal class CurrencyRepository(DolletDbContext dbContext) : ICurrencyRepository
    {
        private readonly DolletDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _dbContext.Currencies.ToListAsync();
        }

        public async Task<Currency?> GetDefaultAsync()
        {
            return await _dbContext.Currencies.FirstOrDefaultAsync(x => x.IsDefault);
        }

        public void Update(Currency currency)
        {
            _dbContext.Currencies.Update(currency);
        }

        public async Task<Dictionary<string, decimal>> GetCurrencyValuesAsync(string code)
        {
            return await _dbContext.CurrencyValues
                .Where(x => x.CodeTo == code)
                .ToDictionaryAsync(x => x.CodeTo, x => x.Value);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Currencies.AnyAsync();
        }

        public void AddMany(IEnumerable<Currency> currencies)
        {
            _dbContext.Currencies.AddRange(currencies);
        }

        public void AddManyValues(IEnumerable<CurrencyValue> currencyValues)
        {
            _dbContext.CurrencyValues.AddRange(currencyValues);
        }

        public async Task DeleteValuesAsync()
        {
            await _dbContext.CurrencyValues.ExecuteDeleteAsync();
        }
    }
}
