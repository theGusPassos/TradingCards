using OpenSearch.Client;
using TradingCards;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
