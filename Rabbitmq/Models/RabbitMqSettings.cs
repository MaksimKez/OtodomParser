using System.ComponentModel.DataAnnotations;

namespace Rabbitmq.Models;

public class RabbitMqSettings
{
    public const string ConfigName = "RabbitMQ";

    public string HostName { get; set; } = "localhost";

    [Range(1, 65535)]
    public int Port { get; set; } = 5672;

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";

    public string Queue { get; set; } = "otodom-listings";
}
