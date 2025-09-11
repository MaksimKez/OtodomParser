using Application.Abstractions;
using Polly;

namespace Infrastructure.Client;

//todo add pagination
// max page size is 72
// if page num > exsiting pages => first page is given
// if page size is not (24, 36, 48, 72) => 36 page size is given
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
