using Application.Abstractions;
using Polly;

namespace Infrastructure.Client;

public class OtodomClient(HttpClient httpClient, ResiliencePipeline<HttpResponseMessage> resiliencePipeline) : IOtodomClient
{
    public async Task<string> GetPageContentAsync(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var response = await resiliencePipeline.ExecuteAsync(
            async ct => await httpClient.GetAsync(path, ct),
            CancellationToken.None);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
