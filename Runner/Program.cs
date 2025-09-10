using System.Text;
using Application.Abstractions;
using Infrastructure.Extensions;
using Infrastructure.Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register Infrastructure client and resilience
builder.Services.AddInfrastructure(builder.Configuration);

var host = builder.Build();

await using var scope = host.Services.CreateAsyncScope();
var client = scope.ServiceProvider.GetRequiredService<IOtodomClient>();

// Arguments: [0] relative path to fetch, [1] output file path
var path = "pl/wyniki/wynajem/mieszkanie/cala-polska?ownerTypeSingleSelect=ALL";
var output = args.Length > 1 ? args[1] : Path.Combine(AppContext.BaseDirectory, "otodom_response.html");

Console.WriteLine($"Fetching: {path}");
var content = await client.GetPageContentAsync(path);

var parsed = await new Parser(new Extractor()).ParseListingsAsync(content);
foreach (var item in parsed)
{
    Console.WriteLine(item.Url);
}

// Ensure directory exists
var outDir = Path.GetDirectoryName(output);
if (!string.IsNullOrWhiteSpace(outDir) && !Directory.Exists(outDir))
{
    Directory.CreateDirectory(outDir);
}

await File.WriteAllTextAsync(output, content, Encoding.UTF8);
Console.WriteLine($"Saved to: {output}");
