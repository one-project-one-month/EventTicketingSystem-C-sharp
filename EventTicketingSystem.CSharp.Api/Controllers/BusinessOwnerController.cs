using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Business Owner")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BusinessOwnerController : ControllerBase
{
    private readonly BL_BusinessOwner _blBusinessOwner;
    private readonly ExportService _exportService;

    public BusinessOwnerController(BL_BusinessOwner blBusinessOwner, ExportService exportService)
    {
        _blBusinessOwner = blBusinessOwner;
        _exportService = exportService;
    }

    [HttpGet("List")]
    public async Task<IActionResult> List()
    {
        return Ok(await _blBusinessOwner.List());
    }

    [HttpGet("Edit/{ownerCode}")]
    public async Task<IActionResult> Edit(string ownerCode)
    {
        return Ok(await _blBusinessOwner.Edit(ownerCode));
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] BusinessOwnerCreateRequestModel requestModel)
    {
        return Ok(await _blBusinessOwner.Create(requestModel));
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] BusinessOwnerUpdateRequestModel requestModel)
    {
        return Ok(await _blBusinessOwner.Update(requestModel));
    }

    [HttpPost("Delete/{ownerCode}")]
    public async Task<IActionResult> Delete(string ownerCode)
    {
        return Ok(await _blBusinessOwner.Delete(ownerCode));
    }


    [HttpPost("Export")]
    public async Task<IActionResult> Export(BusinessOwnerExportRequestModel requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.BusinessOwnerList),
                    "text/csv",
                    "Business_Owner.csv"),
                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.BusinessOwnerList, "Business Owner"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Business_Owner.xlsx"),
                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.BusinessOwnerList, "Business Owner"),
                    "application/pdf",
                    "Business_Owner.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }
}