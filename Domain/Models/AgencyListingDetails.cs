using System.Text.Json.Serialization;

namespace Domain.Models;

public class AgencyListingDetails
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } // "AGENCY"

    [JsonPropertyName("brandingVisible")]
    public bool BrandingVisible { get; set; }

    [JsonPropertyName("highlightedAds")]
    public bool HighlightedAds { get; set; }

    [JsonPropertyName("enhancedBrandingFeatures")]
    public List<string> EnhancedBrandingFeatures { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }
}