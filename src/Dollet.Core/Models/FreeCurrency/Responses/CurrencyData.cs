using System.Text.Json.Serialization;

namespace Dollet.Core.Models.FreeCurrency.Responses
{
    public class CurrencyData
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol_native")]
        public string Symbol_native { get; set; }

        [JsonPropertyName("decimal_digits")]
        public int Decimal_digits { get; set; }

        [JsonPropertyName("rounding")]
        public int Rounding { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name_plural")]
        public string Name_plural { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}