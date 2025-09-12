using Application.Abstractions.Rabbitmq;
using Application.Abstractions.Rabbitmq.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rabbitmq.Connections;
using Rabbitmq.Models;
using Rabbitmq.Publishers;

namespace Rabbitmq.Extensions;

public static class ServiceCollectionInfrastructureExtensions
{
    public static IServiceCollection AddRabbitMqInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.ConfigName));

        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqJsonPublisher>();
        services.AddScoped<IListingPublisherService, ListingPublisherService>();

        return services;
    }
}
