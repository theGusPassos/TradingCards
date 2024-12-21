using System.Text.Json;
using System.Text.Json.Serialization;
using TradingCards.Cards;

namespace TradingCards.Converters
{
    public class CardsConverter(CardTypeRegistry registry) : JsonConverter<List<CardBase>>
    {
        readonly CardTypeRegistry registry = registry;
        readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverterWithAttributeSupport() },
        };

        public override List<CardBase>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);

            var response = new List<CardBase>();

            var hits = document.RootElement.ValueKind == JsonValueKind.Array
                ? document.RootElement
                : document.RootElement.GetProperty("hits").GetProperty("hits");

            foreach (var hit in hits.EnumerateArray())
            {
                var index = hit.GetProperty("_index").GetString()!;
                var type = registry.GetType(index);
                var cardJson = hit.GetProperty("_source").GetRawText();
                response.Add((CardBase)JsonSerializer.Deserialize(cardJson, type, serializerOptions)!);
            }

            return response;
        }

        public override void Write(Utf8JsonWriter writer, List<CardBase> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}
