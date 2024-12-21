using System.Runtime.Serialization;

namespace TradingCards.Cards
{
    public abstract class CardBase
    {
        public virtual CardType Type { get; set; }
        public required string Id { get; set; }
        public required string Name { get; set; }
    }

    public enum CardType
    {
        [EnumMember(Value = "Mtg")]
        MTG,

        [EnumMember(Value = "Lorcana")]
        LORCANA,
    }
}
