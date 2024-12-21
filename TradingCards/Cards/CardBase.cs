namespace TradingCards.Cards
{
    public abstract class CardBase
    {
        public virtual string Type { get; set; } = string.Empty;
        public required string Id { get; set; }
        public required string Name { get; set; }
    }
}
