using OpenSearch.Client;
using System.Text.Json;
using System.Text.Json.Serialization;
using TradingCards.Cards;

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

            var indicesResponse = await client.Indices.ExistsAsync(Constants.IndicesNames.MTG_CARDS, ct: stoppingToken);
            if (!indicesResponse.Exists)
            {
                await client.Indices.CreateAsync(Constants.IndicesNames.MTG_CARDS, r => r.Map(m => m.AutoMap<MtgCard>()), ct: stoppingToken);
                await client.Indices.CreateAsync(Constants.IndicesNames.LORCANA_CARDS, r => r.Map(m => m.AutoMap<LorcanaCard>()), ct: stoppingToken);

                JsonSerializerOptions deserializationOptions = new()
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    Converters = { new JsonStringEnumConverterWithAttributeSupport() },
                };

                using var lorcanaReader = new StreamReader(File.OpenRead($"{Directory.GetCurrentDirectory()}/data/lorcana-cards.json"));
                var lorcanaCardsFile = await lorcanaReader.ReadToEndAsync(stoppingToken);
                var lorcanaCards = JsonSerializer.Deserialize<LorcanaCard[]>(lorcanaCardsFile, deserializationOptions);
                await client.IndexManyAsync(lorcanaCards, Constants.IndicesNames.LORCANA_CARDS, cancellationToken: stoppingToken);

                using var mtgReader = new StreamReader(File.OpenRead($"{Directory.GetCurrentDirectory()}/data/mtg-cards.json"));
                var mtgFile = await mtgReader.ReadToEndAsync(stoppingToken);
                var mtgCards = JsonSerializer.Deserialize<MtgCard[]>(mtgFile, deserializationOptions);
                await client.IndexManyAsync(mtgCards, Constants.IndicesNames.MTG_CARDS, cancellationToken: stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting card loader");
        }
    }
}
