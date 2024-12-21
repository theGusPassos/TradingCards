using TradingCards.Cards;

namespace TradingCards.Controllers.Filters
{
    public class LorcanaCardFilter : FilterBase
    {
        public int? InkCost { get; set; }
        public LorcanaRarity? Rarity { get; set; }
    }
}
