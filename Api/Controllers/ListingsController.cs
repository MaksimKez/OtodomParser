using Application.Abstractions.Rabbitmq;
using Application.Services.Interfaces;
using Domain.Models.Specs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController(IOtodomService otodomService, IListingPublisherService publisherService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? createdWithin = null)
    {
        if (createdWithin is not (1 or 3 or 7))
        {
            var listings = await otodomService.FetchListingsAsync(null);
            return Ok(listings);
        }

        if (createdWithin is not (1 or 3 or 7))
        {
            return BadRequest("createdWithin must be one of: 1, 3, 7");
        }

        var specs = new BaseSpecifications { DaysSinceCreated = createdWithin };
        var filtered = await otodomService.FetchListingsAsync(specs);
        return Ok(filtered);
    }

    [HttpPost("publish")]
    public async Task<IActionResult> Publish([FromQuery] int? createdWithin = null, CancellationToken cancellationToken = default)
    {
        object[]? specs = null;
        if (createdWithin is not null)
        {
            if (createdWithin is not (1 or 3 or 7))
                return BadRequest("createdWithin must be one of: 1, 3, 7");

            specs = [new BaseSpecifications { DaysSinceCreated = createdWithin }];
        }

        await publisherService.PublishListingsAsync(specs, cancellationToken);
        return Accepted();
    }
}

