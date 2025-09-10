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
        var sb = new StringBuilder();
        sb.Append("pl/wyniki/");

        if (specs != null)
        {
            foreach (var spec in specs)
            {
                if (spec == null) continue;
                _baseHandler.Handle(spec, sb);
            }
        }

        var path = NormalizePath(sb.ToString());

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
        if (parts.Count <= 0) return path;
        
        var daysParts = parts.Where(p => p.StartsWith("daysSinceCreated=", StringComparison.OrdinalIgnoreCase)).ToList();
        if (daysParts.Count > 0)
        {
            foreach (var d in daysParts)
                parts.Remove(d);
            parts.AddRange(daysParts);
        }

        var rebuilt = string.Join('&', parts);
        path = basePart + rebuilt;

        return path;
    }
}