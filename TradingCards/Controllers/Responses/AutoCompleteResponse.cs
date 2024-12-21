using TradingCards.Cards;

namespace TradingCards.Controllers.Responses;

public class AutoCompleteResponse
{
    public required List<CardBase> Cards { get; set; }
}
