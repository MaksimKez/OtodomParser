using System.Text.Json.Serialization;

namespace Domain.Models;

public class LocationDetails
{
    [JsonPropertyName("mapDetails")]
    public MapDetails MapDetails { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("reverseGeocoding")]
    public ReverseGeocoding ReverseGeocoding { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
