using Dollet.Core.Models.FreeCurrency.Responses;

namespace Dollet.Core.Abstractions.Services
{
    public interface IFreeCurrencyService
    {
        Task<Dictionary<string, CurrencyData>> GetCurrenciesAsync();
        Task<Dictionary<string, decimal>> GetCurrencyValuesAsync(string? currencyName = null);
    }
}
