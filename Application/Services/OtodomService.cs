using Application.Services.Interfaces;
using Domain.Models.Common;
using Domain.Models.Specs;
using System.Text;
using Application.Abstractions;
using Application.Services.ChainOfSpecHandlers;

namespace Application.Services;

public class OtodomService : IOtodomService
{
    private readonly IOtodomClient _client;
    private readonly IParser _parser;
    private readonly BaseSpecificationsHandler _baseHandler;

    public OtodomService(
        IOtodomClient client,
        IParser parser,
        BaseSpecificationsHandler baseHandler,
        DefaultSpecificationsHandler defaultHandler,
        ExactSpecificationsHandler exactHandler)
    {
        _client = client;
        _parser = parser;
        _baseHandler = baseHandler;

        //base -> default -> exact
        _baseHandler
            .SetNext(defaultHandler)
            .SetNext(exactHandler);
    }

    public async Task<IEnumerable<ListingCommon>> FetchListingsAsync(params object[] specs)
    {
        /*var sb = new StringBuilder();
        sb.Append("pl/wyniki/");

        if (specs != null)
        {
            foreach (var spec in specs)
            {
                if (spec == null) continue;
                _baseHandler.Handle(spec, sb);
            }
        }*/

        var pathWithFilters = "pl/wyniki/wynajem/mieszkanie/mazowieckie/warszawa/warszawa/warszawa?limit=36&priceMin=2000&priceMax=5000&areaMin=20&areaMax=50&roomsNumber=%5BTWO%2CTHREE%5D&daysSinceCreated=1&floors=%5BFIRST%5D&by=DEFAULT&direction=DESC";

        var pathWithoutFilters = "pl/wyniki/wynajem/mieszkanie/mazowieckie/warszawa/warszawa/warszawa?limit=36&by=DEFAULT&direction=DESC";
        
        
        var html = await _client.GetPageContentAsync(pathWithFilters);
        
        //todo if-se for filtered/nonfiltered
        
        var listings = await _parser.ParseFilteredListingsAsync(html);
        return listings;
    }
}
