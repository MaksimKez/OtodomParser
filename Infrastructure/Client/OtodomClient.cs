using Application.Abstractions;

namespace Infrastructure.Client;

public class OtodomClient(IOtodomApi otodomApi) : IOtodomClient
{
    public async Task<string> GetPageContentAsync(string path)
    {
        
    }
}