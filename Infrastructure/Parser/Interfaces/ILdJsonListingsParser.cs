using Domain.Models.Common;

namespace Infrastructure.Parser.Interfaces;

public interface ILdJsonListingsParser
{
    Task<IEnumerable<ListingCommon>> ParseAsync(string html);
}
