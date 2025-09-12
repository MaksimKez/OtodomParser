using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Abstractions;
using Domain.FilteredAppliedParsingModel;
using Domain.Models.Common;
using Domain.ParsingModels;
using HtmlAgilityPack;
using Infrastructure.Parser.Interfaces;

namespace Infrastructure.Parser;

public class Parser : IParser
{
    private readonly ILdJsonListingsParser _ldJsonParser;
    private readonly IFilteredListingsParser _filteredParser;

    public Parser(ILdJsonListingsParser ldJsonParser, IFilteredListingsParser filteredParser)
    {
        _ldJsonParser = ldJsonParser;
        _filteredParser = filteredParser;
    }

    public Task<IEnumerable<ListingCommon>> ParseListingsAsync(string listingText)
    {
        return _ldJsonParser.ParseAsync(listingText);
    }

    public Task<IEnumerable<ListingCommon>> ParseFilteredListingsAsync(string html)
    {
        return _filteredParser.ParseAsync(html);
    }
}