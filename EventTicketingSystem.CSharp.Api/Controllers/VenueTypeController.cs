using EventTicketingSystem.CSharp.Domain.Features.VenueType;
using EventTicketingSystem.CSharp.Domain.Models.Features.VenueType;
using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers
{
    [Tags("Venue Type")]
    [Route("api/[controller]")]
    [ApiController]
    public class VenueTypeController : ControllerBase
    {
        private readonly BL_VenueType _blVenueType;
        private readonly ExportService _exportService;

        public VenueTypeController(BL_VenueType venueType, ExportService exportService)
        {
            _blVenueType = venueType;
            _exportService = exportService;
        }

        [HttpGet("List")]
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            return Ok(await _blVenueType.List());
        }

        [HttpGet("Edit/{venueTypeCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string venueTypeCode)
        {
            return Ok(await _blVenueType.Edit(venueTypeCode));
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] VenueTypeCreateRequestModel requestModel)
        {
            return Ok(await _blVenueType.Create(requestModel));
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] VenueTypeUpdateRequestModel requestModel)
        {
            return Ok(await _blVenueType.Update(requestModel));
        }

        [HttpPost("Delete/{venueTypeCode}")]
        [Authorize]
        public async Task<IActionResult> Delete(string venueTypeCode)
        {
            return Ok(await _blVenueType.Delete(venueTypeCode));
        }

        [HttpPost("Export")]
        public async Task<IActionResult> Export(VenueTypeExportRequestModel requestModel)
        {
            try
            {
                return requestModel.Format.ToLower() switch
                {
                    "csv" => File(
                        await _exportService.ExportToCsv(requestModel.VenueTypeList),
                        "text/csv",
                        "Venue_Type.csv"),
                    "xlsx" or "excel" => File(
                        await _exportService.ExportToExcel(requestModel.VenueTypeList, "Venue Type"),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Venue_Type.xlsx"),
                    "pdf" => File(
                        await _exportService.ExportToPdf(requestModel.VenueTypeList, "Venue Type"),
                        "application/pdf",
                        "Venue_Type.pdf"),

                    _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Export failed: {ex.Message}");
            }
        }
    }
}
