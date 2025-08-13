using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Event Category")]
[Route("api/[controller]")]
[ApiController]
public class EventCategoryController : ControllerBase
{
    private readonly BL_EventCategory _blEventCategory;
    private readonly ExportService _exportService;

    public EventCategoryController(BL_EventCategory blEventCategory, ExportService exportService)
    {
        _blEventCategory = blEventCategory;
        _exportService = exportService;
    }

    [HttpGet("List")]
    [AllowAnonymous]
    public async Task<IActionResult> List()
    {
        return Ok(await _blEventCategory.List());
    }

    [HttpGet("Edit/{eventCategoryCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> Edit(string eventCategoryCode)
    {
        return Ok(await _blEventCategory.Edit(eventCategoryCode));
    }

    [HttpPost("Create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] EventCategoryCreateRequestModel requestModel)
    {
        return Ok(await _blEventCategory.Create(requestModel));
    }

    [HttpPost("Update")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] EventCategoryUpdateRequestModel requestModel)
    {
        return Ok(await _blEventCategory.Update(requestModel));
    }

    [HttpPost("Delete/{eventCategoryCode}")]
    [Authorize]
    public async Task<IActionResult> Delete(string eventCategoryCode)
    {
        return Ok(await _blEventCategory.Delete(eventCategoryCode));
    }

    [HttpPost("Export")]
    public async Task<IActionResult> Export(EventCategoryExportRequestModel requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.EventCateogryList),
                    "text/csv",
                    "Event_Category.csv"),
                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.EventCateogryList, "Event Category"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Event_Category.xlsx"),
                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.EventCateogryList, "Event Category"),
                    "application/pdf",
                    "Event_Category.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }
}