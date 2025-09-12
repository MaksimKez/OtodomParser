namespace Infrastructure.Models;

public class PollySettings
{
    public const string ConfigName = "PollySettings";
    public int RetryCount { get; set; } = 3;
    public double RetryBaseDelaySeconds { get; set; } = 0.5;
    public double TimeoutSeconds { get; set; } = 10;
}