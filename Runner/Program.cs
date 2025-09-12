using Application.Extensions;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Models.Specs;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var host = builder.Build();

await using var scope = host.Services.CreateAsyncScope();
var otodom = scope.ServiceProvider.GetRequiredService<IOtodomService>();

var baseSpecs = new BaseSpecifications
{
    TransactionType = TransactionType.RENT,
    EstateType = EstateType.FLAT,
    Localization = "mazowieckie/warszawa/warszawa/warszawa",
    DaysSinceCreated = 1
};

var defaultSpecs = new DefaultSpecifications
{
    PriceMin = 2000,
    PriceMax = 5000,
    AreaMin = 20,
    AreaMax = 50,
    RoomNumber = new[] { RoomNumber.TWO, RoomNumber.THREE },
    Floors = new[] { FloorNumber.FIRST }
};

Console.WriteLine("Fetching listings using OtodomService...");
var listings = await otodom.FetchListingsAsync(baseSpecs, defaultSpecs);

int i = 0;
foreach (var item in listings)
{
    Console.WriteLine($"[{++i}] {item.Url} | Price: {item.Price} | Area: {item.AreaMeterSqr} | Rooms: {item.Rooms}");
}

Console.WriteLine($"Total listings: {i}");
