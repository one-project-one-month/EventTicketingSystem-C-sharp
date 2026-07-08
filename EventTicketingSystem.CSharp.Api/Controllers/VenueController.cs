namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Venue")]
[Route("api/[controller]")]
[ApiController]
public class VenueController : ControllerBase
{   
    private readonly BL_Venue _blVenue;
    public VenueController(BL_Venue blService)
    {
        _blVenue = blService;
    }

    [HttpGet("List")]
    [AllowAnonymous]
    public async Task<IActionResult> List()
    {
        var data = await _blVenue.List();
        return Ok(data);
    }

    [HttpGet("Edit/{venueCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> Edit(string venueCode)
    {
        var data = await _blVenue.Edit(venueCode);
        return Ok(data);
    }

    [HttpPost("Create")]
    [Consumes("multipart/form-data")]
    [Authorize]
    public async Task<IActionResult> Create(VenueCreateRequestModel requestModel)
    {
        var data = await _blVenue.Create(requestModel);
        return Ok(data);
    }

    [HttpPost("Update")]
    [Authorize]
    public async Task<IActionResult> Update(VenueUpdateRequestModel requestModel)
    {
        var data = await _blVenue.Update(requestModel);
        return Ok(data);
    }

    [HttpPost("Delete/{venueCode}")]
    [Authorize]
    public async Task<IActionResult> Delete(string venueCode)
    {
        var data = await _blVenue.Delete(venueCode);
        return Ok(data);
    }

    [HttpGet("UserVenueList/{pageNo}")]
    [AllowAnonymous]
    public async Task<IActionResult> UserVenueList(int pageNo)
    {
        var data = await _blVenue.UserVenueList(pageNo);
        return Ok(data);
    }

    [HttpGet("UserVenueDetail/{venueCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> UserVenueDetail(string venueCode)
    {
        var data = await _blVenue.UserVenueDetail(venueCode);
        return Ok(data);
    }
}
