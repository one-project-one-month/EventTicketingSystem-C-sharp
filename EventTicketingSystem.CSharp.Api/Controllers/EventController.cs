using EventTicketingSystem.CSharp.Domain.Features.UserEvent;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Event")]
[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly BL_Event _blEvent;
    private readonly BL_User_Event _blUserEvent;
    private readonly ExportService _exportService;

    public EventController(BL_Event blEvent, BL_User_Event bL_User_Event,ExportService exportService)
    {
        _blEvent = blEvent;
        _blUserEvent = bL_User_Event;
        _exportService = exportService;
    }

    [HttpGet("List/{pageNo}")]
    [AllowAnonymous]
    public async Task<IActionResult> List(int pageNo)
    {
        var data = await _blEvent.List(pageNo);
        return Ok(data);
    }

    [HttpGet("Edit/{eventCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> Edit(string eventCode)
    {
        var data = await _blEvent.Edit(eventCode);
        return Ok(data);
    }

    //[HttpGet("Top3Events")]
    //[AllowAnonymous]
    //public async Task<IActionResult> GetTop3Events()
    //{
    //    var data = await _blUserEvent.GetTop3Events();
    //    return Ok(data);
    //}

    //[HttpGet("UserList/{pageNo}")]
    //[AllowAnonymous]
    //public async Task<IActionResult> UserEventList(int pageNo)
    //{
    //    var data = await _blUserEvent.GetEventList(pageNo);
    //    return Ok(data);
    //}

    [HttpGet("UserEvents/{pageNo}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserEventList(int pageNo)
    {
        var data = await _blUserEvent.GetUserEventList(pageNo);
        return Ok(data);
    }

    [HttpGet("UserEventDetails/{eventCode}")]
    public async Task<IActionResult> GetUserEventDetails(string eventCode)
    {
        var data = await _blUserEvent.GetUserEventDetails(eventCode);
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
    
    [HttpPost("Export")]
    public async Task<IActionResult> Export(EventExportRequestModel requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.EventList),
                    "text/csv",
                    "Events.csv"),
                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.EventList, "Events"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Events.xlsx"),
                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.EventList, "Events"),
                    "application/pdf",
                    "Events.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
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