using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Domain.Models.Specs;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController(IOtodomService otodomService) : ControllerBase
{
    // GET: /api/listings?createdWithin=1|3|7
    // createdWithin is optional: if omitted -> no date filter; allowed values: 1 (24h), 3, 7 days
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int createdWithin = 0)
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
}
