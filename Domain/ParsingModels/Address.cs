using System.Text.Json.Serialization;

namespace Domain.ParsingModels;

public class Address
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string AddressCountry { get; set; }

    public string AddressLocality { get; set; }

    public string AddressRegion { get; set; }

    public string StreetAddress { get; set; }
}
