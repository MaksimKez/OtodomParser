using Domain.Models;

namespace Application.Abstractions;

public interface IOtodomClient
{
    Task<string> GetPageContentAsync(string path);
}