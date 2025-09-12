using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rabbitmq.Models;

namespace Rabbitmq.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IListingsPublisher, RabbitMqListingsPublisher>();
        return services;
    }
}
