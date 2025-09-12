using System.Text.Json.Serialization;

namespace Domain.ParsingModels;

public class Offer
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string Availability { get; set; }

    public string Image { get; set; }

    public decimal Price { get; set; }

    public string PriceCurrency { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public ItemOffered ItemOffered { get; set; }
}
