using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradingCards.Converters
{
    public static class Config
    {
        public static JsonSerializerOptions deserializationOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters = { new JsonStringEnumConverterWithAttributeSupport() },
        };

        public static JsonSerializerOptions fromOpenSearchOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverterWithAttributeSupport() },
        };
    }
}
