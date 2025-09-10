using Application.Abstractions;
using Domain.Models;
using Domain.Models.Common;

namespace Infrastructure;

public class Parser : IParser
{
    public Task<IEnumerable<ListingCommon>> ParseListingsAsync(string listingText)
    {
        throw new NotImplementedException();
    }
}
