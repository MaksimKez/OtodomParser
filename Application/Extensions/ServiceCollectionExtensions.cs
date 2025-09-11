using Application.Services;
using Application.Services.ChainOfSpecHandlers;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<BaseSpecificationsHandler>();
        services.AddTransient<DefaultSpecificationsHandler>();
        services.AddTransient<ExactSpecificationsHandler>();

        services.AddScoped<ISpecHandlerChainFactory, SpecHandlerChainFactory>();
        services.AddScoped<IListingPathProvider, ListingPathProvider>();

        services.AddScoped<IOtodomService, OtodomService>();

        return services;
    }
}
