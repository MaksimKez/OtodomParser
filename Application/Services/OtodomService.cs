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
    private readonly ISpecHandlerChainFactory _chainFactory;
    private readonly IListingPathProvider _pathProvider;

    public OtodomService(
        IOtodomClient client,
        IParser parser,
        ISpecHandlerChainFactory chainFactory,
        IListingPathProvider pathProvider)
    {
        _client = client;
        _parser = parser;
        _chainFactory = chainFactory;
        _pathProvider = pathProvider;
        
        // chain construction moved to factory to respect SRP and DIP
        _ = _chainFactory.Create();
    }

    public async Task<IEnumerable<ListingCommon>> FetchListingsAsync(params object[] specs)
    {
        var pathWithFilters = _pathProvider.BuildFilteredPath(specs);
        
        //var pathWithoutFilters = _pathProvider.BuildNonFilteredPath(specs);
        
        var html = await _client.GetPageContentAsync(pathWithFilters);
        
        //todo if-se for filtered/nonfiltered
        
        var listings = await _parser.ParseFilteredListingsAsync(html);
        return listings;
    }
}
