using System.Text.Json.Serialization;

namespace Domain.Models;

public class Address
{
    [JsonPropertyName("street")]
    public Street Street { get; set; }

    [JsonPropertyName("city")]
    public City City { get; set; }

    [JsonPropertyName("province")]
    public Province Province { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}
