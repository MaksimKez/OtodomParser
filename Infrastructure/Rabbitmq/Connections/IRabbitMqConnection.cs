using RabbitMQ.Client;

namespace Rabbitmq.Connections;

public interface IRabbitMqConnection
{
    IConnection CreateConnection();
    IModel CreateChannel();
}
