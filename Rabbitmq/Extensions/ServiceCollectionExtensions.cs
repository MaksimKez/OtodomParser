using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rabbitmq.BackgroundServices;
using Rabbitmq.BackgroundServices.Delays;
using Rabbitmq.BackgroundServices.Delays.Interfaces;
using Rabbitmq.Models;

namespace Rabbitmq.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDelayStrategy, FixedDelayStrategy>();
        services.AddSingleton<IListingsPublisher, RabbitMqListingsPublisher>();
        services.AddHostedService<PublishingBackgroundService>();
        return services;
    }
}
