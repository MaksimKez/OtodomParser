namespace Application.Abstractions.Rabbitmq;

public interface IListingPublisherService
{
    Task PublishListingsAsync(object[]? specs = null, CancellationToken cancellationToken = default);
}
