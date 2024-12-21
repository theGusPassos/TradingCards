using OpenSearch.Client;
using OpenSearch.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TradingCards.Cards;
using TradingCards.Constants;

namespace TradingCards;

public class CardLoader(OpenSearchClient client, ILogger<CardLoader> logger) : BackgroundService
{
    readonly OpenSearchClient client = client;
    readonly ILogger logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var response = await client.PingAsync(ct: stoppingToken);
            if (!response.IsValid)
            {
                throw new Exception("open search connection error");
            }

            var indicesResponse = await client.Indices.ExistsAsync(Constants.Indices.MTG_CARDS, ct: stoppingToken);
            if (indicesResponse.Exists)
            {
                await client.Indices.CreateAsync(Constants.Indices.MTG_CARDS, r => r.Map(m => m.AutoMap<MtgCard>()), ct: stoppingToken);
                await client.Indices.CreateAsync(Constants.Indices.LORCANA_CARDS, r => r.Map(m => m.AutoMap<LorcanaCard>()), ct: stoppingToken);

                var deserializationOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                };
                deserializationOptions.Converters.Add(new JsonStringEnumConverterWithAttributeSupport());

                using var lorcanaReader = new StreamReader(File.OpenRead($"{Directory.GetCurrentDirectory()}/data/lorcana-cards.json"));
                var lorcanaCardsFile = await lorcanaReader.ReadToEndAsync(stoppingToken);
                var lorcanaCards = JsonSerializer.Deserialize<LorcanaCard[]>(lorcanaCardsFile, deserializationOptions);
                await client.IndexManyAsync(lorcanaCards, Constants.Indices.LORCANA_CARDS, cancellationToken: stoppingToken);

                using var mtgReader = new StreamReader(File.OpenRead($"{Directory.GetCurrentDirectory()}/data/mtg-cards.json"));
                var mtgFile = await mtgReader.ReadToEndAsync(stoppingToken);
                var mtgCards = JsonSerializer.Deserialize<MtgCard[]>(mtgFile, deserializationOptions);
                await client.IndexManyAsync(mtgCards, Constants.Indices.LORCANA_CARDS, cancellationToken: stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting card loader");
        }
    }
}
