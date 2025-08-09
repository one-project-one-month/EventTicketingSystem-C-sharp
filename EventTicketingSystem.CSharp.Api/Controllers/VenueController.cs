using EventTicketingSystem.CSharp.Domain.Models.Features.VenueType;
using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Venue")]
[Route("api/[controller]")]
[ApiController]
public class VenueController : ControllerBase
{   
    private readonly BL_Venue _blVenue;
    private readonly ExportService _exportService;

    public VenueController(BL_Venue blService, ExportService exportService)
    {
        _blVenue = blService;
        _exportService = exportService;
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


    [HttpPost("Export")]
    public async Task<IActionResult> Export(VenueExportRequestModle requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.VenueList),
                    "text/csv",
                    "Venue.csv"),
                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.VenueList, "Venue"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Venue.xlsx"),
                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.VenueList, "Venue"),
                    "application/pdf",
                    "Venue.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }
}