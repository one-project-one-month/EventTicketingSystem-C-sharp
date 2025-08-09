using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Business Email")]
[Route("api/[controller]")]
[ApiController]
public class BusinessEmailController : ControllerBase
{
    private readonly BL_BusinessEmail _bl_BusinessEmail;
    private readonly ExportService _exportService;

    public BusinessEmailController(BL_BusinessEmail bl_BusinessEmail, ExportService exportService)
    {
        _bl_BusinessEmail = bl_BusinessEmail;
        _exportService = exportService;
    }

    [HttpGet("List")]
    [Authorize]
    public async Task<IActionResult> List()
    {
        var data = await _bl_BusinessEmail.List();
        return Ok(data);
    }

    [HttpGet("Edit/{businessEmailCode}")]
    [Authorize]
    public async Task<IActionResult> Edit(string businessEmailCode)
    {
        var data = await _bl_BusinessEmail.Edit(businessEmailCode);
        return Ok(data);
    }

    [HttpPost("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] BusinessEmailCreateRequestModel requestModel)
    {
        var data = await _bl_BusinessEmail.Create(requestModel);
        return Ok(data);
    }

    [HttpPost("Export")]
    public async Task<IActionResult> Export(BusinessEmailExportRequestModel requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.BusinessEmailList),
                    "text/csv",
                    "Business_Email.csv"),
                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.BusinessEmailList, "Business Email"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Business_Email.xlsx"),
                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.BusinessEmailList, "Business Email"),
                    "application/pdf",
                    "Business_Email.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }
}