using Microsoft.Extensions.Options;
using Rabbitmq.Connections;
using Rabbitmq.Models;
using Rabbitmq.Publishers;
using System.Net;
using Application.Abstractions;
using Application.Abstractions.Rabbitmq;
using Application.Abstractions.Rabbitmq.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Rabbitmq.Extensions;

public static class ServiceCollectionInfrastructureExtensions
{
    public static IServiceCollection AddRabbitMqInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqJsonPublisher>();
        services.AddScoped<IListingPublisherService, ListingPublisherService>();

        return services;
    }
}
