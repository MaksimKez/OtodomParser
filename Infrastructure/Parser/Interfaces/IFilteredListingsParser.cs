using Domain.Models.Common;

namespace Infrastructure.Parser.Interfaces;

public interface IFilteredListingsParser
{
    Task<IEnumerable<ListingCommon>> ParseAsync(string html);
}
