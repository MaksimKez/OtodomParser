using System.Net;
using System.Net.Http;
using Application.Abstractions;
using Infrastructure.Client;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOtodomClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OtodomApiSettings>(configuration.GetSection(OtodomApiSettings.ConfigName));
        services.Configure<PollySettings>(configuration.GetSection(PollySettings.ConfigName));

        services.AddSingleton<ResiliencePipeline<HttpResponseMessage>>(sp =>
        {
            var polly = sp.GetRequiredService<IOptions<PollySettings>>().Value;

            var retryOptions = new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = Math.Max(0, polly.RetryCount),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(Math.Max(0.0, polly.RetryBaseDelaySeconds)),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(resp =>
                    {
                        var status = (int)resp.StatusCode;
                        return status == 429 || status >= 500;
                    })
            };

            var builder = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(retryOptions)
                .AddTimeout(TimeSpan.FromSeconds(Math.Max(0.0, polly.TimeoutSeconds)));

            return builder.Build();
        });

        services.AddHttpClient<IOtodomClient, OtodomClient>((sp, httpClient) =>
        {
            var api = sp.GetRequiredService<IOptions<OtodomApiSettings>>().Value;
            if (string.IsNullOrWhiteSpace(api.BaseAddress))
                throw new InvalidOperationException("OtodomApi:BaseAddress is not configured");

            httpClient.BaseAddress = new Uri(api.BaseAddress, UriKind.Absolute);
            httpClient.DefaultRequestHeaders.Accept.Clear();

            // Emulate a modern browser
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Accept.ParseAdd(
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-GB,en;q=0.9");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            httpClient.DefaultRequestHeaders.Referrer = new Uri(api.BaseAddress);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer(),
        });

        return services;
    }
}
