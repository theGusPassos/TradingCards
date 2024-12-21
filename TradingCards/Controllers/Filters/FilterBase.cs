using OpenSearch.Client;
using TradingCards.Cards;

namespace TradingCards.Controllers.Filters;

public class FilterBase
{
    public string? Name { get; set; }
    public CardType? Type { get; set; }

    public virtual Task<ISearchResponse<object>> Filter(OpenSearchClient client)
    {
        return client.SearchAsync<object>(s => s.Index("*").Query(q => q.Bool(b =>
        {
            if (Name != null)
            {
                b.Should(s => s.Fuzzy(f => f.Field("name").Value(Name)));
            }

            return b;
        })));
    }
}
