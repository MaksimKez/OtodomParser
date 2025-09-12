namespace Rabbitmq.BackgroundServices.Delays.Interfaces;

public interface IDelayStrategy
{
    Task DelayAsync(CancellationToken cancellationToken);
}