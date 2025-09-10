using System.Text.Json.Serialization;

namespace Domain.ParsingModels;

public class Root
{
    [JsonPropertyName("@context")]
    public string Context { get; set; }

    [JsonPropertyName("@graph")]
    public List<GraphItem> Graph { get; set; }
}
