using System.Text.Json.Serialization;

namespace Domain.Models;

public class ReverseGeocoding
{
    [JsonPropertyName("locations")]
    public List<BasicLocationObject> Locations { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
