using System.Text.Json.Serialization;

namespace Domain.Models;

public class Money
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}