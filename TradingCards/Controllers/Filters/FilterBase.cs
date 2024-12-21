using OpenSearch.Client;
using TradingCards.Cards;

namespace TradingCards.Controllers.Filters
{
    public class FilterBase
    {
        public string? Name { get; set; }
        public CardType? Type { get; set; }

        public virtual void Filter(OpenSearchClient client)
        {
        }
    }
}
