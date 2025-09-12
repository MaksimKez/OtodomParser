using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rabbitmq.BackgroundServices.Delays.Interfaces;

namespace Rabbitmq.BackgroundServices;

public class PublishingBackgroundService
    (IServiceProvider serviceProvider,
        IDelayStrategy delayStrategy)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var retryCount = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Publishing listings to RabbitMQ");
            try
            {
                using var scope = serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IOtodomService>();

                await service.FetchAndPublish();
            }
            catch(Exception)
            {
                if (retryCount > 5)
                {
                    throw new Exception("Failed to publish, too many retries");
                }
                
                retryCount++;
            }
            await delayStrategy.DelayAsync(stoppingToken);
        }
    }
}