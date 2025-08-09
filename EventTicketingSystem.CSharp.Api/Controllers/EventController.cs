namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Event")]
[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly BL_Event _blEvent;

    public EventController(BL_Event blEvent)
    {
        _blEvent = blEvent;
    }

    [HttpGet("List")]
    [AllowAnonymous]
    public async Task<IActionResult> List()
    {
        var data = await _blEvent.List();
        return Ok(data);
    }

    [HttpGet("Edit/{eventCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> Edit(string eventCode)
    {
        var data = await _blEvent.Edit(eventCode);
        return Ok(data);
    }

    [HttpPost("Create")]
    [Authorize]
    public async Task<IActionResult> Create(EventCreateRequestModel requestModel)
    {
        var data = await _blEvent.Create(requestModel);
        return Ok(data);
    }

    [HttpPost("Update")]
    [Authorize]
    public async Task<IActionResult> Update(EventUpdateRequestModel requestModel)
    {
        var data = await _blEvent.Update(requestModel);
        return Ok(data);
    }

    [HttpPost("Delete/{eventCode}")]
    [Authorize]
    public async Task<IActionResult> Delete(string eventCode)
    {
        var data = await _blEvent.Delete(eventCode);
        return Ok(data);
    }
    
    [HttpGet("EventStatusOptions")]
    [Authorize]
    public IActionResult GetEventStatusOptions()
    {
        var result = _blEvent.GetEventStatusOptions(); 

        if (!result.IsSuccess)
            return BadRequest(result); 

        return Ok(result);
    }
}