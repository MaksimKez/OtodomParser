using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Rabbitmq.Models;

namespace Rabbitmq.Connections;

public sealed class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly Lazy<IConnection> _connection;

    public RabbitMqConnection(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _connection = new Lazy<IConnection>(CreateConnectionInternal, isThreadSafe: true);
    }

    public IConnection CreateConnection()
    {
        return _connection.Value;
    }

    public IModel CreateChannel()
    {
        var connection = CreateConnection();
        var channel = connection.CreateModel();

        if (!_settings.DeclareTopology) return channel;
        channel.ExchangeDeclare(exchange: _settings.Exchange, type: ExchangeType.Direct, durable: _settings.Durable, autoDelete: _settings.AutoDelete);
        channel.QueueDeclare(queue: _settings.Queue, durable: _settings.Durable, exclusive: false, autoDelete: _settings.AutoDelete);
        channel.QueueBind(queue: _settings.Queue, exchange: _settings.Exchange, routingKey: _settings.RoutingKey);
        channel.BasicQos(0, _settings.PrefetchCount, global: false);

        return channel;
    }

    private IConnection CreateConnectionInternal()
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost,
            DispatchConsumersAsync = true
        };

        return factory.CreateConnection();
    }

    public void Dispose()
    {
        if (_connection.IsValueCreated)
        {
            try { _connection.Value.Dispose(); } catch { /* ignore */ }
        }
    }
}
