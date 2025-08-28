using System.Text.Json.Serialization;

namespace Domain.Models;

public class BasicLocationObject
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("locationLevel")]
    public string LocationLevel { get; set; } // "voivodeship", "city_or_village", "district", "residential"

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
