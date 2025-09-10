using Domain.Models.Common;

namespace Application.Abstractions;

public interface IParser
{
    Task<IEnumerable<ListingCommon>> ParseListingsAsync(string html);
}
