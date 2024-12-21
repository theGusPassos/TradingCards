using Microsoft.AspNetCore.Mvc;
using OpenSearch.Client;
using OpenSearch.Net;
using System.Text.Json;
using TradingCards.Cards;
using TradingCards.Controllers.Filters;
using TradingCards.Controllers.Responses;
using TradingCards.Converters;

namespace TradingCards.Controllers;

[ApiController]
[Route("[controller]")]
public class CardsController(OpenSearchClient client, CardTypeRegistry cardRegistry) : ControllerBase
{
    readonly OpenSearchClient client = client;
    readonly JsonSerializerOptions openSearchSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new CardsConverter(cardRegistry) },
    };

    [HttpGet("autocomplete")]
    public async Task<IActionResult> AutoComplete(string query)
    {
        var search = new
        {
            query = new
            {
                multi_match = new
                {
                    query,
                    type = "bool_prefix",
                    fields = new string[] { "name" },
                }
            },
            size = 10,
        };

        var searchResponse = await client.LowLevel.SearchAsync<StringResponse>(PostData.Serializable(search));
        return Ok(new AutoCompleteResponse
        {
            Cards = JsonSerializer.Deserialize<List<CardBase>>(searchResponse.Body, openSearchSerializerOptions)!
        });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Filter([ModelBinder(BinderType = typeof(FilterConverter))] FilterBase filter)
    {
        var searchResponse = await filter.Filter(client);
        var serializedCards = JsonSerializer.Serialize(searchResponse.Hits.Select(h => new { _index = h.Index, _source = h.Source }));
        return Ok(new FilterResponse { Cards = JsonSerializer.Deserialize<List<CardBase>>(serializedCards, openSearchSerializerOptions)! });
    }
}
