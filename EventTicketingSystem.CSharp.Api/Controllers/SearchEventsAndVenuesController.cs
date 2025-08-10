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
    public IActionResult SearchEventsAndVenues(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return BadRequest("Search term cannot be null or empty.");
        }

        var result = _bl_SearchEventsAndVenues.SearchEventsAndVenues(searchTerm).Result;

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        return Ok(result);
    }

    [HttpGet("BetweenDate")]
    public IActionResult SearchEventsByDate(DateTime StartDate, DateTime EndDate)
    {
        if (StartDate.IsNullOrEmpty())
        {
            return BadRequest("Search date cannot be null or empty.");
        }

        var result = _bl_SearchEventsAndVenues.SearchEventsByDate(StartDate, EndDate).Result;

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        return Ok(result);
    }

    [HttpGet("BetweenAmount")]
    public IActionResult SearchEventsByAmount(decimal FromAmount, decimal ToAmount)
    {
        if (FromAmount <= 0 || ToAmount <= 0)
        {
            return BadRequest("Search amount cannot be less than or equal to zero.");
        }

        var result = _bl_SearchEventsAndVenues.SearchEventsByAmountAsync(FromAmount, ToAmount).Result;

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        return Ok(result);
    }
}