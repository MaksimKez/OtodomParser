using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions;
using Domain.Models.Common;
using HtmlAgilityPack;

namespace Runner;

public class Parser : IParser
{
    public Task<IEnumerable<ListingCommon>> ParseListingsAsync(string listingText)
    {
        var listings = ParseHtml(listingText);
        return Task.FromResult<IEnumerable<ListingCommon>>(listings);
    }

    private static List<ListingCommon> ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // достаём блок JSON-LD
        var jsonNode = doc.DocumentNode
            .SelectSingleNode("//script[@type='application/ld+json']");

        if (jsonNode == null)
            return new List<ListingCommon>();

        var json = jsonNode.InnerText;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // десериализация во временную модель
        var root = JsonSerializer.Deserialize<Root>(json, options);

        var offers = root?.Graph?
            .FirstOrDefault(g => g.Type == "Product")?
            .Offers?.Offers;

        if (offers == null)
            return new List<ListingCommon>();

        // маппинг во внутреннюю модель
        return offers.Select(o => new ListingCommon
        {
            Id = Guid.NewGuid(),
            Price = o.Price,
            AreaMeterSqr = o.ItemOffered?.FloorSize?.Value ?? 0,
            Rooms = o.ItemOffered?.NumberOfRooms ?? 0,
            Floor = ExtractFloor(o.ItemOffered?.Description),
            IsFurnished = ExtractIsFurnished(o.ItemOffered?.Description),
            PetsAllowed = ExtractPets(o.ItemOffered?.Description),
            HasBalcony = ExtractBalcony(o.ItemOffered?.Description),
            HasAppliances = ExtractAppliances(o.ItemOffered?.Description),
            Url = o.Url,
            CreatedAt = DateTime.UtcNow,
            ImageLink = o.Image
        }).ToList();
    }

    #region Helpers (парсинг из текста description)
    private static int ExtractFloor(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return 0;

        // ищем "I piętro", "2 piętro", "parter"
        if (description.Contains("parter", StringComparison.OrdinalIgnoreCase))
            return 0;

        var parts = description.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (int.TryParse(part.Replace("I", "1"), out var floor))
                return floor;
        }

        return 0;
    }

    private static bool ExtractIsFurnished(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("umeblowane", StringComparison.OrdinalIgnoreCase)
               || description.Contains("meble", StringComparison.OrdinalIgnoreCase)
               || description.Contains("wyposażone", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ExtractPets(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("zwierzęta", StringComparison.OrdinalIgnoreCase)
               || description.Contains("pies", StringComparison.OrdinalIgnoreCase)
               || description.Contains("kot", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ExtractBalcony(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("balkon", StringComparison.OrdinalIgnoreCase)
               || description.Contains("taras", StringComparison.OrdinalIgnoreCase)
               || description.Contains("loggia", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ExtractAppliances(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("AGD", StringComparison.OrdinalIgnoreCase)
               || description.Contains("pralka", StringComparison.OrdinalIgnoreCase)
               || description.Contains("lodówka", StringComparison.OrdinalIgnoreCase)
               || description.Contains("zmywarka", StringComparison.OrdinalIgnoreCase);
    }
    #endregion
}

#region DTO Models (JSON-LD)
public class Root
{
    [JsonPropertyName("@context")]
    public string Context { get; set; }

    [JsonPropertyName("@graph")]
    public List<GraphItem> Graph { get; set; }
}

public class GraphItem
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string Url { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public AggregateOffer Offers { get; set; }
}

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

public class ItemOffered
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string Description { get; set; }

    public Address Address { get; set; }

    public int NumberOfRooms { get; set; }

    public FloorSize FloorSize { get; set; }
}

public class Address
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string AddressCountry { get; set; }

    public string AddressLocality { get; set; }

    public string AddressRegion { get; set; }

    public string StreetAddress { get; set; }
}

public class FloorSize
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public decimal Value { get; set; }

    public string UnitCode { get; set; }
}
#endregion