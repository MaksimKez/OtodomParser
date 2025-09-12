using System.Text.Json;
using System.Text.RegularExpressions;
using Domain.FilteredAppliedParsingModel;
using Domain.Models.Common;
using HtmlAgilityPack;
using Infrastructure.Parser.Interfaces;

namespace Infrastructure.Parser;

public class FilteredListingsParser(IListingUtilities utils) : IFilteredListingsParser
{
    public Task<IEnumerable<ListingCommon>> ParseAsync(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

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
            Rooms = utils.MapRooms(i.RoomsNumber),
            Floor = utils.MapFloor(i.FloorNumber),
            Url = utils.BuildAbsoluteUrl(i.Href, lang),
            CreatedAt = i.CreatedAtFirst,
            ImageLink = utils.BuildAbsoluteUrl(i.Images.FirstOrDefault()?.Large ?? "", lang),
            IsFurnished = false,
            PetsAllowed = false,
            HasBalcony = false,
            HasAppliances = false
        }).ToList();

        return Task.FromResult<IEnumerable<ListingCommon>>(listings);
    }
}
