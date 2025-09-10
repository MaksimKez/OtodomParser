using System.ComponentModel.Design;
using System.Text.Json;
using Application.Abstractions;
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

    private List<ListingCommon> ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var jsonNode = doc.DocumentNode
            .SelectSingleNode("//script[@type='application/ld+json']");

        if (jsonNode == null)
        {
            throw new Exception();
            // Could be that the page rendered via JS or structure changed; return empty list gracefully
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