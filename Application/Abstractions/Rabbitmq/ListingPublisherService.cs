using Application.Abstractions.Rabbitmq.Interfaces;
using Application.Services.Interfaces;

namespace Application.Abstractions.Rabbitmq;

public sealed class ListingPublisherService(IOtodomService otodomService, IMessagePublisher publisher) : IListingPublisherService
{
    public async Task PublishListingsAsync(object[]? specs = null, CancellationToken cancellationToken = default)
    {
        var listings = await otodomService.FetchListingsAsync(specs);
        await publisher.PublishAsync(listings, cancellationToken: cancellationToken);
    }
}
