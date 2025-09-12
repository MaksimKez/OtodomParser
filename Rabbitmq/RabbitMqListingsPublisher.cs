using System.Text;
using System.Text.Json;
using Application.Abstractions;
using Domain.Models.Common;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Rabbitmq.Models;

namespace Rabbitmq;

public class RabbitMqListingsPublisher(IOptions<RabbitMqSettings> options) : IListingsPublisher
{
    private readonly RabbitMqSettings _settings = options.Value;

    public Task PublishAsync(IEnumerable<ListingCommon> listings, CancellationToken cancellationToken = default)
    {
        var list = listings?.ToList() ?? [];
        if (list.Count == 0) return Task.CompletedTask;

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            DispatchConsumersAsync = true
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _settings.Queue,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
        
        foreach (var listing in list)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(listing, options));
            var props = channel.CreateBasicProperties();
            props.DeliveryMode = 2;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _settings.Queue,
                                 basicProperties: props,
                                 body: body);
        }

        return Task.CompletedTask;
    }
}
