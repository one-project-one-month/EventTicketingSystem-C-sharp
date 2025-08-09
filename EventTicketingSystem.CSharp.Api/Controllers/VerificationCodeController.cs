using EventTicketingSystem.CSharp.Domain.Services;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Verification Code")]
[Route("api/[controller]")]
[ApiController]
public class VerificationCodeController : ControllerBase
{
    private readonly BL_VerificationCode _vcService;
    private readonly ExportService _exportService;

    public VerificationCodeController(BL_VerificationCode vcService, ExportService exportService)
    {
        _vcService = vcService;
        _exportService = exportService;
    }

    [HttpGet("List")]
    [Authorize]
    public async Task<IActionResult> List()
    {
        return Ok(await _vcService.List());
    }

    [HttpGet("Get/{vcId}")]
    [Authorize]
    public async Task<IActionResult> GetById(string vcId)
    {
        return Ok(await _vcService.GetById(vcId));
    }

    [HttpGet("GetByEmail/{email}")]
    [Authorize]
    public async Task<IActionResult> GetByEmail(string email)
    {
        return Ok(await _vcService.GetByEmail(email));
    }

    [HttpPost("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] VCRequestModel requestModel)
    {
        return Ok(await _vcService.Create(requestModel));
    }

    [HttpPost("VerifyCode")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode([FromBody] VCRequestModel requestModel)
    {
        return Ok(await _vcService.VerifyCode(requestModel));
    }

    [HttpPost("Export")]
    public async Task<IActionResult> Export(VCExportRequestModel requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.VCodeList),
                    "text/csv",
                    "Verification_Code.csv"),
                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.VCodeList, "Verification Code"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Verification_Code.xlsx"),
                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.VCodeList, "Verification Code"),
                    "application/pdf",
                    "Verification_Code.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }
}