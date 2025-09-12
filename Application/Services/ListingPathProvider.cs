using Application.Services.Interfaces;
using System.Text;

namespace Application.Services;

public class ListingPathProvider(ISpecHandlerChainFactory chainFactory) : IListingPathProvider
{
    public string GetFilteredPath()
    {
        return "pl/wyniki/wynajem/mieszkanie/mazowieckie/warszawa/warszawa/warszawa?limit=36&priceMin=2000&priceMax=5000&areaMin=20&areaMax=50&roomsNumber=%5BTWO%2CTHREE%5D&daysSinceCreated=1&floors=%5BFIRST%5D&by=DEFAULT&direction=DESC";
    }

    public string GetNonFilteredPath()
    {
        return "pl/wyniki/wynajem/mieszkanie/mazowieckie/warszawa/warszawa/warszawa?limit=36&by=DEFAULT&direction=DESC";
    }

    public string BuildFilteredPath(params object[]? specs)
    {
        var sb = new StringBuilder();
        sb.Append("pl/wyniki/");

        if (specs is { Length: > 0 })
        {
            var chain = chainFactory.Create();
            foreach (var spec in specs)
            {
                if (spec is null) continue;
                chain.Handle(spec, sb);
            }
        }

        if (!sb.ToString().Contains('?'))
        {
            sb.Append("cala-polska?");
        }

        // Append defaults
        if (sb[^1] != '?' && sb[^1] != '&') sb.Append('&');
        sb.Append("limit=36&by=DEFAULT&direction=DESC");

        return sb.ToString();
    }

    public string BuildNonFilteredPath(params object[]? specs)
    {
        var sb = new StringBuilder("pl/wyniki/wynajem/mieszkanie/cala-polska?");
        sb.Append("limit=36&by=DEFAULT&direction=DESC");
        return sb.ToString();
    }

    public string BuildDaysSinceCreatedOnly(int specs)
        => $"pl/wyniki/wynajem/mieszkanie/cala-polska?daysSinceCreated={specs.ToString()}&by=DEFAULT&direction=DESC";
}
