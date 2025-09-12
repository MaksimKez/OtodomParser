using Rabbitmq.BackgroundServices.Delays.Interfaces;

namespace Rabbitmq.BackgroundServices.Delays;

public class FixedDelayStrategy : IDelayStrategy
{
    private const int _delayHours = 6;
    public Task DelayAsync(CancellationToken cancellationToken)
        => Task.Delay(_delayHours * 60 * 60 * 1000, cancellationToken);
}