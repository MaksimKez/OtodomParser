using System.Text.Json.Serialization;

namespace Domain.ParsingModels;

public class AggregateOffer
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string BusinessFunction { get; set; }

    public string HighPrice { get; set; }

    public string LowPrice { get; set; }

    public string PriceCurrency { get; set; }

    public string OfferCount { get; set; }

    public List<Offer> Offers { get; set; }
}
