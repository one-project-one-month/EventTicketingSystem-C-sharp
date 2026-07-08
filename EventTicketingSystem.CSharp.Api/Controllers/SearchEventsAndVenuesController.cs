using EventTicketingSystem.CSharp.Shared;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Search Menu")]
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class SearchEventsAndVenuesController : ControllerBase
{
    private readonly BL_SearchEventsAndVenues _bl_SearchEventsAndVenues;

    public SearchEventsAndVenuesController(BL_SearchEventsAndVenues bl_SearchEventsAndVenues)
    {
        _bl_SearchEventsAndVenues = bl_SearchEventsAndVenues;
    }

    [HttpGet]
    public async Task<IActionResult> SearchEventsAndVenues(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return BadRequest("Search term cannot be null or empty.");
        }

        var result = await _bl_SearchEventsAndVenues.SearchEventsAndVenues(searchTerm);
        return Ok(result);
    }

    //[HttpGet("{StartDate, EndDate}")]
    [HttpGet("BetweenDate")]
    public async Task<IActionResult> SearchEventsByDate(DateTime StartDate, DateTime EndDate)
    {
        if (StartDate.IsNullOrEmpty())
        {
            return BadRequest("Search date cannot be null or empty.");
        }

        var result = await _bl_SearchEventsAndVenues.SearchEventsByDate(StartDate, EndDate);
        return Ok(result);
    }

    [HttpGet("BetweenAmount")]
    public async Task<IActionResult> SearchEventsByAmount(decimal FromAmount, decimal ToAmount)
    {
        if (FromAmount <= 0 || ToAmount <= 0)
        {
            return BadRequest("Search amount cannot be less than or equal to zero.");
        }

        var result = await _bl_SearchEventsAndVenues.SearchEventsByAmountAsync(FromAmount, ToAmount);
        return Ok(result);
    }
}
