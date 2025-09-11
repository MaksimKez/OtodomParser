using Application.Services.Interfaces;

namespace Application.Services;

public class ListingPathProvider(ISpecHandlerChainFactory chainFactory) : IListingPathProvider
{
    public string GetFilteredPath()
    {
        // Keep existing hardcoded path as-is to preserve behavior
        return "pl/wyniki/wynajem/mieszkanie/mazowieckie/warszawa/warszawa/warszawa?limit=36&priceMin=2000&priceMax=5000&areaMin=20&areaMax=50&roomsNumber=%5BTWO%2CTHREE%5D&daysSinceCreated=1&floors=%5BFIRST%5D&by=DEFAULT&direction=DESC";
    }

    public string GetNonFilteredPath()
    {
        // Keep existing hardcoded path as-is to preserve behavior
        return "pl/wyniki/wynajem/mieszkanie/mazowieckie/warszawa/warszawa/warszawa?limit=36&by=DEFAULT&direction=DESC";
    }

    public string BuildFilteredPath(params object[] specs)
    {
        // For future: build path using handlers; for now, preserve current behavior
        // by returning the existing hardcoded filtered path regardless of specs.

        // Example of how it will work once enabled:
        // var sb = new StringBuilder();
        // sb.Append("pl/wyniki/");
        // if (specs != null)
        // {
        //     var chain = _chainFactory.Create();
        //     foreach (var spec in specs)
        //     {
        //         if (spec == null) continue;
        //         chain.Handle(spec, sb);
        //     }
        // }
        // return sb.ToString();

        return GetFilteredPath();
    }

    public string BuildNonFilteredPath(params object[] specs)
    {
        // For future: similar to BuildFilteredPath, but for non-filtered scenario.
        // Keep behavior unchanged for now.
        return GetNonFilteredPath();
    }
}
