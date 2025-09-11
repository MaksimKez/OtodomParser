using Application.Abstractions;

namespace Infrastructure.Client;

public class OtodomClient : IOtodomClient
{
    public Task<string> GetPageContentAsync(string query)
    {
        throw new NotImplementedException();
    }
}