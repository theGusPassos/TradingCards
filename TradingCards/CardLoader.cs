using OpenSearch.Client;

namespace TradingCards;

public class CardLoader(OpenSearchClient client) : BackgroundService
{
    readonly OpenSearchClient client = client;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var response = await client.PingAsync(ct: stoppingToken);
        if (!response.IsValid)
        {
            throw new Exception("open search connection error");
        }
    }
}
