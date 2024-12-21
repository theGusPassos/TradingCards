using TradingCards.Cards;

namespace TradingCards.Controllers.Filters
{
    public class MtgCardFilter : FilterBase
    {
        public MtgColor? Color { get; set; }
        public MtgRarity? Rarity { get; set; }
    }
}
