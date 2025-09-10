using Application.Services.Interfaces;
using Domain.Models.Common;
using Domain.Models.Specs;
using System.Text;
using Application.Abstractions;
using Application.Services.ChainOfSpecHandlers;

namespace Application.Services;

public class OtodomService : IOtodomService
{
    private readonly IOtodomClient _client;
    private readonly IParser _parser;
    private readonly BaseSpecificationsHandler _baseHandler;
    private readonly DefaultSpecificationsHandler _defaultHandler;
    private readonly ExactSpecificationsHandler _exactHandler;

    public OtodomService(
        IOtodomClient client,
        IParser parser,
        BaseSpecificationsHandler baseHandler,
        DefaultSpecificationsHandler defaultHandler,
        ExactSpecificationsHandler exactHandler)
    {
        _client = client;
        _parser = parser;
        _baseHandler = baseHandler;
        _defaultHandler = defaultHandler;
        _exactHandler = exactHandler;

        //base -> default -> exact
        _baseHandler
            .SetNext(_defaultHandler)
            .SetNext(_exactHandler);
    }

    public async Task<IEnumerable<ListingCommon>> FetchListingsAsync(params object[] specs)
    {
        /*var sb = new StringBuilder();
        sb.Append("pl/wyniki/");

        if (specs != null)
        {
            foreach (var spec in specs)
            {
                if (spec == null) continue;
                _baseHandler.Handle(spec, sb);
            }
        }

        var path = NormalizePath(sb.ToString());*/

        var path = "pl/wyniki/wynajem/mieszkanie/cala-polska?by=latest&ownerTypeSingleSelect=ALL";

        var html = await _client.GetPageContentAsync(path);
        var listings = await _parser.ParseListingsAsync(html);
        return listings;
    }


    private static string NormalizePath(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Path is empty after building from specifications", nameof(raw));

        var path = raw.Replace("//", "/");

        var idxQuestion = path.IndexOf('?'); 
        if (idxQuestion < 0)
        {
            var idxEq = path.IndexOf('=');
            if (idxEq > 0)
            {
                var idxSlashBefore = path.LastIndexOf('/', idxEq);
                var keyStart = idxSlashBefore >= 0 ? idxSlashBefore + 1 : 0;
                path = path.Insert(keyStart, "?");
            }
        }

        if (path.EndsWith("&")) path = path.TrimEnd('&');
        if (path.EndsWith("?")) path = path.TrimEnd('?');

        idxQuestion = path.IndexOf('?');
        if (idxQuestion < 0 || idxQuestion >= path.Length - 1) return path;
        var basePart = path[..(idxQuestion + 1)];
        var query = path[(idxQuestion + 1)..];

        var parts = query.Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        if (parts.Count <= 0)
        {
            // If there are no parts, we still want to enforce defaults
            parts = new List<string>();
        }

        // Parse into a lookup of key -> last value occurrence
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in parts)
        {
            var idx = p.IndexOf('=');
            if (idx <= 0) continue;
            var k = p[..idx];
            var v = p[(idx + 1)..];
            map[k] = v; // keep last occurrence
        }

        // Inject defaults if missing
        if (!map.ContainsKey("limit")) map["limit"] = "36";
        if (!map.ContainsKey("by")) map["by"] = "DEFAULT";
        if (!map.ContainsKey("direction")) map["direction"] = "DESC";

        // Rebuild in EXACT desired order
        var order = new[]
        {
            "limit",
            "priceMin",
            "priceMax",
            "areaMin",
            "areaMax",
            "roomsNumber",
            "daysSinceCreated",
            "floors",
            "by",
            "direction"
        };

        var rebuiltParts = new List<string>();
        foreach (var key in order)
        {
            if (map.TryGetValue(key, out var val))
            {
                rebuiltParts.Add($"{key}={val}");
            }
        }

        var rebuilt = string.Join('&', rebuiltParts);
        path = basePart + rebuilt;

        return path;
    }
}