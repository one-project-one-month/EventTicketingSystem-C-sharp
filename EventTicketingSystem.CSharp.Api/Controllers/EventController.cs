using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Event")]
[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly BL_Event _blEvent;
    private readonly ExportService _exportService;

    public EventController(BL_Event blEvent, ExportService exportService)
    {
        _blEvent = blEvent;
        _exportService = exportService;
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