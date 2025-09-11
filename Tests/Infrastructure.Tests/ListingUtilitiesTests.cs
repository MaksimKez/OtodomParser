using FluentAssertions;
using Infrastructure.Parser;
using Xunit;

namespace Infrastructure.Tests;

public class ListingUtilitiesTests
{
    private readonly ListingUtilities _utils = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void BuildAbsoluteUrl_ShouldReturnNull_WhenHrefIsNullOrWhitespace(string? href)
    {
        var result = _utils.BuildAbsoluteUrl(href);
        result.Should().BeNull();
    }

    [Fact]
    public void BuildAbsoluteUrl_ShouldReturnSame_WhenAlreadyAbsolute()
    {
        var href = "https://example.com/path";
        var result = _utils.BuildAbsoluteUrl(href);
        result.Should().Be(href);
    }

    [Theory]
    [InlineData("/pl/ad/something", "https://www.otodom.pl/pl/oferta/something")]
    [InlineData("pl/ad/something", "https://www.otodom.pl/pl/oferta/something")]
    [InlineData("/ad/something", "https://www.otodom.pl/oferta/something")]
    public void BuildAbsoluteUrl_ShouldNormalizeAdToOferta(string href, string expected)
    {
        var result = _utils.BuildAbsoluteUrl(href, "pl");
        result.Should().Be(expected);
    }

    [Fact]
    public void BuildAbsoluteUrl_ShouldReplaceLangToken()
    {
        var result = _utils.BuildAbsoluteUrl("/[lang]/ad/item", "pl");
        result.Should().Be("https://www.otodom.pl/pl/oferta/item");
    }

    [Theory]
    [InlineData("ONE", 1)]
    [InlineData("TWO", 2)]
    [InlineData("THREE", 3)]
    [InlineData("FOUR", 4)]
    [InlineData("unknown", 0)]
    public void MapRooms_ShouldMapKnownValues(string rooms, int expected)
    {
        _utils.MapRooms(rooms).Should().Be(expected);
    }

    [Theory]
    [InlineData("GROUND", 0)]
    [InlineData("FIRST", 1)]
    [InlineData("SECOND", 2)]
    [InlineData("THIRD", 3)]
    [InlineData("unknown", -1)]
    public void MapFloor_ShouldMapKnownValues(string floor, int expected)
    {
        _utils.MapFloor(floor).Should().Be(expected);
    }
}
