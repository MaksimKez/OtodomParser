using Domain.Models;

namespace Application.Abstractions;

public interface IOtodomClient
{
    Task<IEnumerable<AdvertListItem>> GetAdvertsAsync(string query);
}