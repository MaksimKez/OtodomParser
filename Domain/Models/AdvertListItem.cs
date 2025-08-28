using System.Text.Json.Serialization;

namespace Domain.Models;

public class AdvertListItem
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("estate")]
    public string Estate { get; set; } // "FLAT", "HOUSE", etc.

    [JsonPropertyName("development")]
    public object Development { get; set; } // может быть null

    [JsonPropertyName("developmentId")]
    public int DevelopmentId { get; set; }

    [JsonPropertyName("developmentTitle")]
    public string DevelopmentTitle { get; set; }

    [JsonPropertyName("developmentUrl")]
    public string DevelopmentUrl { get; set; }

    [JsonPropertyName("transaction")]
    public string Transaction { get; set; } // "RENT", "SALE"

    [JsonPropertyName("location")]
    public LocationDetails Location { get; set; }

    [JsonPropertyName("images")]
    public List<AdImage> Images { get; set; }

    [JsonPropertyName("totalPossibleImages")]
    public int TotalPossibleImages { get; set; }

    [JsonPropertyName("isExclusiveOffer")]
    public bool IsExclusiveOffer { get; set; }

    [JsonPropertyName("isPrivateOwner")]
    public bool IsPrivateOwner { get; set; }

    [JsonPropertyName("isPromoted")]
    public bool IsPromoted { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("agency")]
    public AgencyListingDetails Agency { get; set; }

    [JsonPropertyName("openDays")]
    public string OpenDays { get; set; }

    [JsonPropertyName("totalPrice")]
    public Money TotalPrice { get; set; }

    [JsonPropertyName("rentPrice")]
    public Money RentPrice { get; set; }

    [JsonPropertyName("priceFromPerSquareMeter")]
    public Money PriceFromPerSquareMeter { get; set; }

    [JsonPropertyName("pricePerSquareMeter")]
    public Money PricePerSquareMeter { get; set; }

    [JsonPropertyName("areaInSquareMeters")]
    public double AreaInSquareMeters { get; set; }

    [JsonPropertyName("terrainAreaInSquareMeters")]
    public double? TerrainAreaInSquareMeters { get; set; }

    [JsonPropertyName("roomsNumber")]
    public string RoomsNumber { get; set; } // "ONE", "TWO", "THREE", etc.

    [JsonPropertyName("hidePrice")]
    public bool HidePrice { get; set; }

    [JsonPropertyName("floorNumber")]
    public string FloorNumber { get; set; } // "FIRST", "SECOND", "GROUND", etc.

    [JsonPropertyName("investmentState")]
    public string InvestmentState { get; set; }

    [JsonPropertyName("investmentUnitsAreaInSquareMeters")]
    public double? InvestmentUnitsAreaInSquareMeters { get; set; }

    [JsonPropertyName("peoplePerRoom")]
    public int? PeoplePerRoom { get; set; }

    [JsonPropertyName("dateCreated")]
    public string DateCreated { get; set; }

    [JsonPropertyName("createdAtFirst")]
    public DateTime CreatedAtFirst { get; set; }

    [JsonPropertyName("investmentUnitsNumber")]
    public int? InvestmentUnitsNumber { get; set; }

    [JsonPropertyName("investmentUnitsRoomsNumber")]
    public string InvestmentUnitsRoomsNumber { get; set; }

    [JsonPropertyName("investmentEstimatedDelivery")]
    public DateTime? InvestmentEstimatedDelivery { get; set; }

    [JsonPropertyName("pushedUpAt")]
    public DateTime? PushedUpAt { get; set; }

    [JsonPropertyName("specialOffer")]
    public string SpecialOffer { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("__typename")]
    public string TypeName { get; set; }

    [JsonPropertyName("href")]
    public string Href { get; set; }
}
