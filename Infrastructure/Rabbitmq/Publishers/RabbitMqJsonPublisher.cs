using System.Text.Json;
using Application.Abstractions.Rabbitmq.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Rabbitmq.Connections;
using Rabbitmq.Models;

namespace Rabbitmq.Publishers;

public sealed class RabbitMqJsonPublisher
    (IRabbitMqConnection connection,
        IOptions<RabbitMqSettings> options)
    : IMessagePublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    private readonly RabbitMqSettings _settings = options.Value;

    public Task PublishAsync<T>(T payload, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(payload, JsonOptions);
        var propsPersistent = CreateBasicProperties();

        using var channel = connection.CreateChannel();
        var rk = routingKey ?? _settings.RoutingKey;
        channel.BasicPublish(exchange: _settings.Exchange,
            routingKey: rk,
            basicProperties: propsPersistent,
            body: body);

        return Task.CompletedTask;
    }

    private IBasicProperties CreateBasicProperties()
    {
        using var channel = connection.CreateChannel();
        var props = channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;
        return props;
    }
}
