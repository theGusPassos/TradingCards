using TradingCards.Cards;

namespace TradingCards.Controllers.Responses;

public class FilterResponse
{
    public required List<CardBase> Cards { get; set; }
}
