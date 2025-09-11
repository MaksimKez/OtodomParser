using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Abstractions;
using Domain.FilteredAppliedParsingModel;
using Domain.Models.Common;
using Domain.ParsingModels;
using HtmlAgilityPack;
using Infrastructure.Parser.Interfaces;

namespace Infrastructure.Parser;

public class Parser(IExtractor extractor) : IParser
{ 
    public Task<IEnumerable<ListingCommon>> ParseListingsAsync(string listingText)
    {
        var listings = ParseHtml(listingText);
        return Task.FromResult<IEnumerable<ListingCommon>>(listings);
    }

    private const string OtodomBase = "https://www.otodom.pl";

    public Task<IEnumerable<ListingCommon>> ParseFilteredListingsAsync(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // ищем скрипт, в котором лежит props (фильтрованный JSON)
        var scriptNode = doc.DocumentNode
            .SelectSingleNode("//script[contains(text(), 'props')]");
        if (scriptNode == null)
            return Task.FromResult<IEnumerable<ListingCommon>>(new List<ListingCommon>());

        var jsonText = scriptNode.InnerText;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        FilteredRoot? root;
        try
        {
            root = JsonSerializer.Deserialize<FilteredRoot>(jsonText, options);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to parse JSON from page with filters", ex);
        }

        var lang = "pl";

        var items = root?.Props?.PageProps?.Data?.SearchAds?.Items;
        if (items == null || !items.Any())
            return Task.FromResult<IEnumerable<ListingCommon>>(new List<ListingCommon>());

        var listings = items.Select(i => new ListingCommon
        {
            Id = Guid.NewGuid(),
            Price = i.TotalPrice?.Value ?? 0,
            AreaMeterSqr = (decimal)(i.AreaInSquareMeters),
            Rooms = MapRooms(i.RoomsNumber),
            Floor = MapFloor(i.FloorNumber),
            Url = BuildAbsoluteUrl(i.Href, lang),
            CreatedAt = i.CreatedAtFirst,
            ImageLink = BuildAbsoluteUrl(i.Images.FirstOrDefault()?.Large ?? "", lang),
            IsFurnished = false,
            PetsAllowed = false,
            HasBalcony = false,
            HasAppliances = false
        }).ToList();

        return Task.FromResult<IEnumerable<ListingCommon>>(listings);
    }

    private string BuildAbsoluteUrl(string href, string lang = "pl")
    {
        if (string.IsNullOrWhiteSpace(href))
            return null;

        href = href.Trim();

        if (href.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            href.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return href;
        }

        href = Regex.Replace(href, @"\[(?:lang)\]", lang, RegexOptions.IgnoreCase);

        if (!href.StartsWith("/"))
            href = "/" + href;

        var langEscaped = Regex.Escape(lang);
        href = Regex.Replace(href,
            $@"^/{langEscaped}/ad(?=/|$)",
            $"/{lang}/oferta",
            RegexOptions.IgnoreCase);

        href = Regex.Replace(href, @"/ad/", "/oferta/", RegexOptions.IgnoreCase);

        href = Regex.Replace(href, @"^/ad(?=$|/|\?)", $"/{lang}/oferta", RegexOptions.IgnoreCase);

        return OtodomBase + href;
    }
    private int MapRooms(string rooms)
    {
        return rooms.ToUpper() switch
        {
            "ONE" => 1,
            "TWO" => 2,
            "THREE" => 3,
            "FOUR" => 4,
            _ => 0
        };
    }

    private int MapFloor(string floor)
    {
        return floor.ToUpper() switch
        {
            "GROUND" => 0,
            "FIRST" => 1,
            "SECOND" => 2,
            "THIRD" => 3,
            _ => -1
        };
    }

    private List<ListingCommon> ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var jsonNode = doc.DocumentNode
            .SelectSingleNode("//script[@type='application/ld+json']");

        if (jsonNode == null)
        {
            throw new Exception();
            return new List<ListingCommon>();
        }

        var json = jsonNode.InnerText;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var root = JsonSerializer.Deserialize<Root>(json, options);

        var offers = root?.Graph?
            .FirstOrDefault(g => g.Type == "Product")?
            .Offers?.Offers;

        if (offers == null)
            return new List<ListingCommon>();

        return offers.Select(o => new ListingCommon
        {
            Id = Guid.NewGuid(),
            Price = o.Price,
            AreaMeterSqr = o.ItemOffered?.FloorSize?.Value ?? 0,
            Rooms = o.ItemOffered?.NumberOfRooms ?? 0,
            Floor = extractor.ExtractFloor(o.ItemOffered?.Description ?? "noinfo"),
            IsFurnished = extractor.ExtractIsFurnished(o.ItemOffered?.Description ?? "noinfo"),
            PetsAllowed = extractor.ExtractPets(o.ItemOffered?.Description ?? "noinfo"),
            HasBalcony = extractor.ExtractBalcony(o.ItemOffered?.Description ?? "noinfo"),
            HasAppliances = extractor.ExtractAppliances(o.ItemOffered?.Description ?? "noinfo"),
            Url = o.Url,
            CreatedAt = DateTime.UtcNow,
            ImageLink = o.Image
        }).ToList();
    }
}