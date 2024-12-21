using OpenSearch.Client;
using TradingCards.Cards;
using TradingCards.Constants;

namespace TradingCards.Controllers.Filters;

public class MtgCardFilter : FilterBase
{
    public MtgColor? Color { get; set; }
    public MtgRarity? Rarity { get; set; }

    IEnumerable<Func<QueryContainerDescriptor<object>, QueryContainer>> BuildQuery()
    {
        var queries = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>();

        if (Name != null)
        {
            queries.Add(f => f.Term(t => t.Field("name").Value(Name)));
        }

        if (Color != null)
        {
            queries.Add(f => f.Term(t => t.Field("color").Value(Color)));
        }

        if (Rarity != null)
        {
            queries.Add(f => f.Term(t => t.Field("rarity").Value(Rarity)));
        }

        return queries;
    }

    public override Task<ISearchResponse<object>> Filter(OpenSearchClient client)
    {
        return client.SearchAsync<object>(s => s
            .Index(IndicesNames.MTG_CARDS)
            .Query(q => q.Bool(b => b.Must(
                BuildQuery()
            ))));
    }
}
