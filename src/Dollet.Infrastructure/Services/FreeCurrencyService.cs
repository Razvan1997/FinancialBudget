using Dollet.Core.Abstractions.Services;
using Dollet.Core.Models.FreeCurrency.Responses;
using freecurrencyapi;
using System.Text.Json;

namespace Dollet.Infrastructure.Services
{
    internal class FreeCurrencyService(Freecurrencyapi freecurrencyapi) : IFreeCurrencyService
    {
        private readonly Freecurrencyapi _freecurrencyapi = freecurrencyapi;

        public async Task<Dictionary<string, CurrencyData>> GetCurrenciesAsync()
        {
            var currencies = _freecurrencyapi.Currencies();

            var data = JsonSerializer.Deserialize<JsonDocument>(currencies);
            var allCurrencies = new Dictionary<string, CurrencyData>();

            foreach (var currency in data.RootElement.GetProperty("data").EnumerateObject())
            {
                var currencyData = JsonSerializer.Deserialize<CurrencyData>(currency.Value.GetRawText());
                allCurrencies.Add(currency.Name, currencyData);
            }

            return allCurrencies;
        }

        public async Task<Dictionary<string, decimal>> GetCurrencyValuesAsync(string? currencyName = null)
        {
            var currencies = _freecurrencyapi.Latest(currencyName);

            var data = JsonSerializer.Deserialize<JsonDocument>(currencies);
            var allCurrencies = new Dictionary<string, decimal>();

            foreach (var currency in data.RootElement.GetProperty("data").EnumerateObject())
            {
                var currencyData = JsonSerializer.Deserialize<decimal>(currency.Value.GetRawText());
                allCurrencies.Add(currency.Name, currencyData);
            }

            return allCurrencies;
        }
    }
}
