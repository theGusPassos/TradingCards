using OpenSearch.Client;
using TradingCards.Cards;
using TradingCards.Constants;

namespace TradingCards.Controllers.Filters;

public class MtgCardFilter : FilterBase
{
    public MtgColor? Color { get; set; }
    public MtgRarity? Rarity { get; set; }

    public override Task<ISearchResponse<object>> Filter(OpenSearchClient client)
    {
        return client.SearchAsync<object>(s => s.Index(IndicesNames.MTG_CARDS).Query(q => q.Bool(b =>
        {
            if (Name != null)
            {
                b.Should(s => s.Fuzzy(f => f.Field("name").Value(Name)));
            }

            if (Color != null)
            {
                b.Must(mu => mu.Term(t => t.Field("color").Value(Color)));
            }

            if (Rarity != null)
            {
                b.Must(mu => mu.Term(t => t.Field("rarity").Value(Rarity)));
            }

            return b;
        })));
    }
}
