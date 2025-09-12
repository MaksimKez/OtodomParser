using Domain.Models.Common;

namespace Application.Abstractions;

public interface IParser
{
    Task<IEnumerable<ListingCommon>> ParseListingsAsync(string html);
    Task<IEnumerable<ListingCommon>> ParseFilteredListingsAsync(string html);

}
