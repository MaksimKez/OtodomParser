using System.Text.Json.Serialization;

namespace Domain.Models;

public class City
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
