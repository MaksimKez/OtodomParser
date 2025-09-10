using Domain.Models;
using Domain.Models.Common;

namespace Application.Abstractions;

public interface IParser
{
    Task<IEnumerable<AdvertListItem>> ParseListingsAsync(string listingText);
}
