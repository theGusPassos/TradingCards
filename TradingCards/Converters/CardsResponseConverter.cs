using System.Text.Json;
using System.Text.Json.Serialization;
using TradingCards.Cards;

namespace TradingCards.Converters
{
    public class CardsResponseConverter : JsonConverter<List<CardBase>>
    {
        public override List<CardBase>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, List<CardBase> value, JsonSerializerOptions options)
        {
            foreach (var card in value)
            {
                JsonSerializer.Serialize(writer, card, card.GetType(), options);
            }
        }
    }
}
