using Domain.Models.Common;

namespace Application.Abstractions;

public interface IListingsPublisher
{
    Task PublishAsync(IEnumerable<ListingCommon> listings, CancellationToken cancellationToken = default);
}
