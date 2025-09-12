using Application.Abstractions;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Models.Common;
using FluentAssertions;
using Moq;

namespace Application.Tests;

public class OtodomServiceTests
{
    [Fact]
    public async Task FetchListingsAsync_ShouldBuildPath_CallClient_And_ParseFilteredListings()
    {
        // Arrange
        var client = new Mock<IOtodomClient>(MockBehavior.Strict);
        var parser = new Mock<IParser>(MockBehavior.Strict);
        var pathProvider = new Mock<IListingPathProvider>(MockBehavior.Strict);
        var chainFactory = new Mock<ISpecHandlerChainFactory>(MockBehavior.Strict);

        var expectedPath = "/test/path";
        var html = "<html>stub</html>";
        var parsed = new List<ListingCommon> { new() { Id = Guid.NewGuid(), Price = 1 } } as IEnumerable<ListingCommon>;

        pathProvider.Setup(p => p.BuildFilteredPath(It.IsAny<object[]>())).Returns(expectedPath);
        client.Setup(c => c.GetPageContentAsync(expectedPath)).ReturnsAsync(html);
        parser.Setup(p => p.ParseFilteredListingsAsync(html)).ReturnsAsync(parsed);
        chainFactory.Setup(f => f.Create()).Returns(Mock.Of<Application.Services.ChainOfSpecHandlers.BaseSpecificationsHandler>());

        var sut = new OtodomService(client.Object, parser.Object, chainFactory.Object, pathProvider.Object, null);

        // Act
        var result = await sut.FetchListingsAsync("spec1");

        // Assert
        pathProvider.Verify(p => p.BuildFilteredPath(It.IsAny<object[]>()), Times.Once);
        client.Verify(c => c.GetPageContentAsync(expectedPath), Times.Once);
        parser.Verify(p => p.ParseFilteredListingsAsync(html), Times.Once);

        result.Should().BeEquivalentTo(parsed);
    }
}
