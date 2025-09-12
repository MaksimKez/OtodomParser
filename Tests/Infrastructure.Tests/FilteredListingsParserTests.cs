using FluentAssertions;
using Domain.Models;
using Infrastructure.Parser;
using Infrastructure.Parser.Interfaces;
using Moq;
using Xunit;

namespace Infrastructure.Tests;

public class FilteredListingsParserTests
{
    private static string WrapInHtml(string inner) => $"<html><head></head><body>{inner}</body></html>";

    [Fact]
    public async Task ParseAsync_ShouldReturnEmpty_WhenNoPropsScript()
    {
        var utils = new Mock<IListingUtilities>(MockBehavior.Strict);
        var sut = new FilteredListingsParser(utils.Object);

        var html = WrapInHtml("<div>no props</div>");

        var result = await sut.ParseAsync(html);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ParseAsync_ShouldParseItemsFromPropsJson()
    {
        var utils = new Mock<IListingUtilities>();
        utils.Setup(u => u.MapRooms("TWO")).Returns(2);
        utils.Setup(u => u.MapFloor("FIRST")).Returns(1);
        utils.Setup(u => u.BuildAbsoluteUrl(It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string>((href, lang) => href?.StartsWith("http") == true ? href : $"https://www.otodom.pl/{href?.TrimStart('/')}" );

        var sut = new FilteredListingsParser(utils.Object);

        var item = new AdvertListItem
        {
            Id = 1,
            Title = "t",
            Slug = "s",
            Estate = "FLAT",
            Development = null,
            DevelopmentId = 0,
            DevelopmentTitle = "",
            DevelopmentUrl = "",
            Transaction = "RENT",
            Location = null,
            Images = new List<AdImage> { new() { Large = "/img/1.jpg", Medium = "/img/1m.jpg", TypeName = "img" } },
            TotalPossibleImages = 1,
            IsExclusiveOffer = false,
            IsPrivateOwner = false,
            IsPromoted = false,
            Source = "otodom",
            Agency = null,
            OpenDays = "",
            TotalPrice = new Money { Value = 3000, Currency = "PLN", TypeName = "Money" },
            RentPrice = null,
            PriceFromPerSquareMeter = null,
            PricePerSquareMeter = null,
            AreaInSquareMeters = 40,
            TerrainAreaInSquareMeters = null,
            RoomsNumber = "TWO",
            HidePrice = false,
            FloorNumber = "FIRST",
            InvestmentState = "",
            InvestmentUnitsAreaInSquareMeters = null,
            PeoplePerRoom = null,
            DateCreated = DateTime.UtcNow.ToString("O"),
            CreatedAtFirst = new DateTime(2024,1,1,0,0,0,DateTimeKind.Utc),
            InvestmentUnitsNumber = null,
            InvestmentUnitsRoomsNumber = null,
            InvestmentEstimatedDelivery = null,
            PushedUpAt = null,
            SpecialOffer = "",
            ShortDescription = "",
            TypeName = "Advert",
            Href = "/pl/ad/test"
        };

        var json = System.Text.Json.JsonSerializer.Serialize(new
        {
            props = new
            {
                pageProps = new
                {
                    data = new
                    {
                        searchAds = new
                        {
                            items = new[] { item }
                        }
                    }
                }
            }
        });

        var html = WrapInHtml($"<script>{json}</script>");

        var result = (await sut.ParseAsync(html)).ToList();

        result.Should().HaveCount(1);
        var listing = result[0];
        listing.Price.Should().Be(3000);
        listing.AreaMeterSqr.Should().Be(40);
        listing.Rooms.Should().Be(2);
        listing.Floor.Should().Be(1);
        listing.Url.Should().Be("https://www.otodom.pl/pl/ad/test");
        listing.ImageLink.Should().Be("https://www.otodom.pl/img/1.jpg");
        listing.CreatedAt.Should().Be(item.CreatedAtFirst);
        listing.IsFurnished.Should().BeFalse();
        listing.PetsAllowed.Should().BeFalse();
        listing.HasBalcony.Should().BeFalse();
        listing.HasAppliances.Should().BeFalse();
    }
}
