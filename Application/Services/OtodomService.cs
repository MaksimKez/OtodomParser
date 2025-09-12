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
    private readonly IListingsPublisher _publisher;

    public OtodomService(
        IOtodomClient client,
        IParser parser,
        ISpecHandlerChainFactory chainFactory,
        IListingPathProvider pathProvider,
        IListingsPublisher publisher)
    {
        _client = client;
        _parser = parser;
        _pathProvider = pathProvider;
        _publisher = publisher;
        
        _ = chainFactory.Create();
    }

    public async Task<IEnumerable<ListingCommon>> FetchAndPublish()
    {
        var path = _pathProvider.GetNonFilteredPath();
        var html = await _client.GetPageContentAsync(path);
        var listings = await _parser.ParseListingsAsync(html);
        if (listings is null)
        {
            
        }
        await _publisher.PublishAsync(listings);
        return listings;
    }


    public async Task<IEnumerable<ListingCommon>> FetchListingsAsync(params object[]? specs)
    {
        string path, html;
        if (specs is null)
        {
            path = _pathProvider.GetNonFilteredPath();
            html = await _client.GetPageContentAsync(path);
            var listings = await _parser.ParseListingsAsync(html);
            return listings;
        }

        if (specs[0] is BaseSpecifications)
        {
            var spec = (BaseSpecifications)specs[0];
            path = _pathProvider.BuildDaysSinceCreatedOnly((int)spec.DaysSinceCreated!);
            html = await _client.GetPageContentAsync(path);
            var filtered = await _parser.ParseFilteredListingsAsync(html);
            return filtered;
        }

        path = _pathProvider.BuildFilteredPath(specs);

        html = await _client.GetPageContentAsync(path);

        var result = await _parser.ParseFilteredListingsAsync(html);
        return result;
    }
}
