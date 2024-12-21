using OpenSearch.Client;
using TradingCards.Cards;
using TradingCards.Constants;

namespace TradingCards.Controllers.Filters;

public class LorcanaCardFilter : FilterBase
{
    public int? InkCost { get; set; }
    public LorcanaRarity? Rarity { get; set; }

    public override Task<ISearchResponse<object>> Filter(OpenSearchClient client)
    {
        return client.SearchAsync<object>(s => s.Index(IndicesNames.LORCANA_CARDS).Query(q => q.Bool(b =>
        {
            if (Name != null)
            {
                b.Should(s => s.Fuzzy(f => f.Field("name").Value(Name)));
            }

            if (Rarity != null)
            {
                b.Must(mu => mu.Term(t => t.Field("rarity").Value(Rarity)));
            }

            if (InkCost != null)
            {
                b.Must(mu => mu.Term(t => t.Field("inkCost").Value(Rarity)));
            }

            return b;
        })));
    }
}
