namespace Application.Abstractions.Rabbitmq.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T payload, string? routingKey = null, CancellationToken cancellationToken = default);
}
