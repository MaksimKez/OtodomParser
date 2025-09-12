using System.Text.Json.Serialization;

namespace Domain.ParsingModels;

public class FloorSize
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public decimal Value { get; set; }

    public string UnitCode { get; set; }
}
