using System.Text.Json.Serialization;

namespace Domain.Models;

public class MapDetails
{
    [JsonPropertyName("radius")]
    public int Radius { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
