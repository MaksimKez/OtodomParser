using System.Text.Json.Serialization;

namespace Domain.Models;

public class Street
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
