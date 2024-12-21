using Microsoft.AspNetCore.Mvc;
using OpenSearch.Client;
using OpenSearch.Net;
using System.Text.Json;
using TradingCards.Cards;
using TradingCards.Controllers.Filters;
using TradingCards.Controllers.Responses;
using TradingCards.Converters;

namespace TradingCards.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController(OpenSearchClient client, CardTypeRegistry cardRegistry, ILogger<CardsController> logger) : ControllerBase
    {
        readonly OpenSearchClient client = client;
        readonly CardTypeRegistry cardRegistry = cardRegistry;
        readonly ILogger<CardsController> logger = logger;

        [HttpGet(Name = "autocomplete")]
        public async Task<AutoCompleteResponse> AutoComplete(string query)
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

            var deserializationOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                Converters = { new AutoCompleteResponseConverter(cardRegistry) },
            };

            var searchResponse = await client.LowLevel.SearchAsync<StringResponse>(PostData.Serializable(search));
            return JsonSerializer.Deserialize<AutoCompleteResponse>(searchResponse.Body, deserializationOptions)!;
        }

        [HttpGet("search")]
        public async Task Filter([ModelBinder(BinderType = typeof(FilterConverter))] FilterBase query)
        {
        }
    }
}
