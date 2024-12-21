using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TradingCards.Cards;
using TradingCards.Converters;

namespace TradingCards.Controllers.Responses
{
    public class AutoCompleteResponseConverter(CardTypeRegistry registry) : JsonConverter<AutoCompleteResponse>
    {
        CardTypeRegistry registry = registry;

        public override AutoCompleteResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);

            var response = new AutoCompleteResponse();

            var hits = document.RootElement.GetProperty("hits").GetProperty("hits");
            foreach (var hit in hits.EnumerateArray())
            {
                var index = hit.GetProperty("_index").GetString()!;
                var type = registry.GetType(index);
                var cardJson = hit.GetProperty("_source").GetRawText();
                response.Cards.Add((CardBase) JsonSerializer.Deserialize(cardJson, type, Converters.Config.fromOpenSearchOptions)!);
            }

            return response;
        }

        public override void Write(Utf8JsonWriter writer, AutoCompleteResponse value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
