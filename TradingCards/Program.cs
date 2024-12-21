using OpenSearch.Client;
using TradingCards;
using TradingCards.Cards;
using TradingCards.Constants;
using TradingCards.Controllers.Filters;
using TradingCards.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(s =>
{
    var openSearchSection = s.GetRequiredService<IConfiguration>().GetSection("OpenSearch");
    var settings = new ConnectionSettings(new Uri(openSearchSection.GetValue<string>("Uri")!))
        .BasicAuthentication("admin", openSearchSection.GetValue<string>("Password"))
        // for local requests
        .ServerCertificateValidationCallback((o, cert, chain, errors) => true);

    return new OpenSearchClient(settings);
});

builder.Services.AddSingleton(b =>
{
    var cardTypeRegistry = new CardTypeRegistry();
    cardTypeRegistry.Register<MtgCard>(IndicesNames.MTG_CARDS);
    cardTypeRegistry.Register<LorcanaCard>(IndicesNames.LORCANA_CARDS);
    return cardTypeRegistry;
});

builder.Services.AddSingleton(b =>
{
    var filterTypeRegistry = new FilterTypeRegistry();
    filterTypeRegistry.Register<MtgCardFilter>(CardType.MTG.ToString());
    filterTypeRegistry.Register<LorcanaCard>(CardType.LORCANA.ToString());
    return filterTypeRegistry;
});

builder.Services.AddHostedService<CardLoader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class TradingCardsProgram { }

