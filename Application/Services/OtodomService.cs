using Application.Services.Interfaces;
using Domain.Models.Common;
using Application.Abstractions;
using Domain.Models.Specs;

namespace Application.Services;

public class OtodomService : IOtodomService
{
    private readonly IOtodomClient _client;
    private readonly IParser _parser;
    private readonly IListingPathProvider _pathProvider;

    public OtodomService(
        IOtodomClient client,
        IParser parser,
        ISpecHandlerChainFactory chainFactory,
        IListingPathProvider pathProvider)
    {
        _client = client;
        _parser = parser;
        _pathProvider = pathProvider;
        
        _ = chainFactory.Create();
    }

    public async Task<IEnumerable<ListingCommon>> FetchListingsAsync(params object[]? specs)
    {
        string path, html;
        if (specs is null)
        {
            path = _pathProvider.GetNonFilteredPath();
            html = await _client.GetPageContentAsync(path);
            return await _parser.ParseListingsAsync(html);
        }

        if (specs[0] is BaseSpecifications)
        {
            var spec = (BaseSpecifications)specs[0];
            path = _pathProvider.BuildDaysSinceCreatedOnly((int)spec.DaysSinceCreated!);
            html = await _client.GetPageContentAsync(path);
            return await _parser.ParseFilteredListingsAsync(html);
        }

        path = _pathProvider.BuildFilteredPath(specs);

        html = await _client.GetPageContentAsync(path);
        
        return  await _parser.ParseFilteredListingsAsync(html);
    }
}
