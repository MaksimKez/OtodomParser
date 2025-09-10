using Refit;

namespace Infrastructure.Client;

public interface IOtodomApi
{
    [Get("/")]
    Task<HttpResponseMessage> Get(string path);
}