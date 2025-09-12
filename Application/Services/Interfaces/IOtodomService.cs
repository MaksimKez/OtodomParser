using Domain.Models.Common;
using Domain.Models.Specs;

namespace Application.Services.Interfaces;

public interface IOtodomService
{
    Task FetchAndPublish();
    Task<IEnumerable<ListingCommon>> FetchListingsAsync(params object[]? specs);
}