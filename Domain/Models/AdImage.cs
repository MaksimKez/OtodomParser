using System.Text.Json.Serialization;

namespace Domain.Models;

public class AdImage
{
    [JsonPropertyName("medium")]
    public string Medium { get; set; }

    [JsonPropertyName("large")]
    public string Large { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
