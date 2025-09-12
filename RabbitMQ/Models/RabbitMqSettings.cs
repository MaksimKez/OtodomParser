using System.ComponentModel.DataAnnotations;

namespace Rabbitmq.Models;

public class RabbitMqSettings
{
    public const string ConfigName = "RabbitMq";

    [Required]
    public string HostName { get; init; } = "localhost";

    public int Port { get; init; } = 5672;

    public string UserName { get; init; } = "guest";

    public string Password { get; init; } = "guest";

    public string VirtualHost { get; init; } = "/";

    [Required]
    public string Exchange { get; init; } = "otodom.exchange";

    [Required]
    public string Queue { get; init; } = "otodom.listings";

    public string RoutingKey { get; init; } = "otodom.listings";

    public bool Durable { get; init; } = true;

    public bool AutoDelete { get; init; } = false;

    public bool DeclareTopology { get; init; } = true;

    public bool PublisherConfirms { get; init; } = true;

    public ushort PrefetchCount { get; init; } = 10;
}
