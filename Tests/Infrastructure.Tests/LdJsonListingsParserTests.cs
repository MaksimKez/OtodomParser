using FluentAssertions;
using Infrastructure.Parser;
using Infrastructure.Parser.Interfaces;
using Moq;
using Xunit;

namespace Infrastructure.Tests;

public class LdJsonListingsParserTests
{
    private static string WrapInHtml(string inner) => $"<html><head></head><body>{inner}</body></html>";

    [Fact]
    public async Task ParseAsync_ShouldReturnEmpty_WhenNoLdJsonScript()
    {
        var extractor = new Mock<IExtractor>(MockBehavior.Strict);
        var sut = new LdJsonListingsParser(extractor.Object);

        var html = WrapInHtml("<div>No ld+json here</div>");

        var result = await sut.ParseAsync(html);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ParseAsync_ShouldParseOffersFromLdJson()
    {
        var extractor = new Mock<IExtractor>();
        extractor.Setup(x => x.ExtractFloor(It.IsAny<string>())).Returns(2);
        extractor.Setup(x => x.ExtractIsFurnished(It.IsAny<string>())).Returns(true);
        extractor.Setup(x => x.ExtractPets(It.IsAny<string>())).Returns(false);
        extractor.Setup(x => x.ExtractBalcony(It.IsAny<string>())).Returns(true);
        extractor.Setup(x => x.ExtractAppliances(It.IsAny<string>())).Returns(true);

        var sut = new LdJsonListingsParser(extractor.Object);

        var ldJson = """
        <script type='application/ld+json'>
        {
          "@context": "https://schema.org",
          "@graph": [
            {
              "@type": "Product",
              "offers": {
                "@type": "AggregateOffer",
                "offers": [
                  {
                    "@type": "Offer",
                    "price": 3450,
                    "image": "https://img/1.jpg",
                    "url": "https://www.otodom.pl/offer/1",
                    "itemOffered": {
                      "@type": "Apartment",
                      "description": "Nice flat on second floor",
                      "numberOfRooms": 2,
                      "floorSize": { "@type": "QuantitativeValue", "value": 42 }
                    }
                  }
                ]
              }
            }
          ]
        }
        </script>
        """;
        var html = WrapInHtml(ldJson);

        var result = (await sut.ParseAsync(html)).ToList();

        result.Should().HaveCount(1);
        var item = result[0];
        item.Price.Should().Be(3450);
        item.AreaMeterSqr.Should().Be(42);
        item.Rooms.Should().Be(2);
        item.Floor.Should().Be(2);
        item.IsFurnished.Should().BeTrue();
        item.PetsAllowed.Should().BeFalse();
        item.HasBalcony.Should().BeTrue();
        item.HasAppliances.Should().BeTrue();
        item.Url.Should().Be("https://www.otodom.pl/offer/1");
        item.ImageLink.Should().Be("https://img/1.jpg");
    }
}
