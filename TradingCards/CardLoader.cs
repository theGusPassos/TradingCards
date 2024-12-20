using OpenSearch.Client;

namespace TradingCards;

public class CardLoader : BackgroundService
{
    readonly OpenSearchClient client;

    public CardLoader(IConfiguration configuration)
    {
        var openSearchSection = configuration.GetSection("OpenSearch");
        var settings = new ConnectionSettings(new Uri(openSearchSection.GetValue<string>("Uri")!))
            .BasicAuthentication("admin", openSearchSection.GetValue<string>("Password"))
            // for local requests
            .ServerCertificateValidationCallback((o, cert, chain, errors) => true);

        client = new  OpenSearchClient(settings);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var response = await client.PingAsync(ct: stoppingToken);
        if (!response.IsValid)
        {
            throw new Exception("open search connection error");
        }
    }
}
