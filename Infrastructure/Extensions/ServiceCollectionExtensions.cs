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
        });

        return services;
    }
}
