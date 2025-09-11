using System.Text.RegularExpressions;
using Infrastructure.Parser.Interfaces;

namespace Infrastructure.Parser;

public class ListingUtilities : IListingUtilities
{
    private const string OtodomBase = "https://www.otodom.pl";

    public string BuildAbsoluteUrl(string href, string lang = "pl")
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

    public int MapRooms(string rooms)
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

    public int MapFloor(string floor)
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
}
