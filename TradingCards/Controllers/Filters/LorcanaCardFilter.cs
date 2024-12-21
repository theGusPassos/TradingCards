using OpenSearch.Client;
using TradingCards.Cards;
using TradingCards.Constants;

namespace TradingCards.Controllers.Filters;

public class LorcanaCardFilter : FilterBase
{
    public int? InkCost { get; set; }
    public LorcanaRarity? Rarity { get; set; }

    IEnumerable<Func<QueryContainerDescriptor<object>, QueryContainer>> BuildQuery()
    {
        var queries = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>();

        if (Name != null)
        {
            queries.Add(f => f.Term(t => t.Field("name").Value(Name)));
        }

        if (Rarity != null)
        {
            queries.Add(f => f.Term(t => t.Field("rarity").Value(Rarity)));
        }

        if (InkCost != null)
        {
            queries.Add(f => f.Term(t => t.Field("inkCost").Value(InkCost)));
        }

        return queries;
    }

    public override Task<ISearchResponse<object>> Filter(OpenSearchClient client)
    {
        return client.SearchAsync<object>(s => s
            .Index(IndicesNames.LORCANA_CARDS)
            .Query(q => q.Bool(b => b.Must(
                BuildQuery()
            ))));
    }
}
