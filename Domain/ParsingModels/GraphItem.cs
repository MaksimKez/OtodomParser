using System.Text.Json.Serialization;

namespace Domain.ParsingModels;

public class GraphItem
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string Url { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public AggregateOffer Offers { get; set; }
}
