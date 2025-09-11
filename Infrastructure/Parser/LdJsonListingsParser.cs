using System.Text.Json;
using Application.Abstractions;
using Domain.Models.Common;
using Domain.ParsingModels;
using HtmlAgilityPack;
using Infrastructure.Parser.Interfaces;

namespace Infrastructure.Parser;

public class LdJsonListingsParser : ILdJsonListingsParser
{
    private readonly IExtractor _extractor;

    public LdJsonListingsParser(IExtractor extractor)
    {
        _extractor = extractor;
    }

    public Task<IEnumerable<ListingCommon>> ParseAsync(string html)
    {
        var listings = ParseHtml(html);
        return Task.FromResult<IEnumerable<ListingCommon>>(listings);
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
            Floor = _extractor.ExtractFloor(o.ItemOffered?.Description ?? "noinfo"),
            IsFurnished = _extractor.ExtractIsFurnished(o.ItemOffered?.Description ?? "noinfo"),
            PetsAllowed = _extractor.ExtractPets(o.ItemOffered?.Description ?? "noinfo"),
            HasBalcony = _extractor.ExtractBalcony(o.ItemOffered?.Description ?? "noinfo"),
            HasAppliances = _extractor.ExtractAppliances(o.ItemOffered?.Description ?? "noinfo"),
            Url = o.Url,
            CreatedAt = DateTime.UtcNow,
            ImageLink = o.Image
        }).ToList();
    }
}
