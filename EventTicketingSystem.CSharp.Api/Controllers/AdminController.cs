namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Admin User")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly BL_Admin _blAdmin;
    private readonly ExportService _exportService;

    public AdminController(BL_Admin blAdmin, ExportService exportService)
    {
        _blAdmin = blAdmin;
        _exportService = exportService;
    }

    [HttpGet("List")]
    public async Task<IActionResult> List()
    {
        var data = await _blAdmin.List();
        return Ok(data);
    }

    [HttpGet("Edit/{adminCode}")]
    public async Task<IActionResult> Edit(string adminCode)
    {
        var data = await _blAdmin.Edit(adminCode);
        return Ok(data);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(AdminCreateRequestModel requestModel)
    {
        var data = await _blAdmin.Create(requestModel);
        return Ok(data);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(AdminUpdateRequestModel requestModel)
    {
        var data = await _blAdmin.Update(requestModel);
        return Ok(data);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(AdminDeleteRequestModel requestModel)
    {
        var data = await _blAdmin.Delete(requestModel);
        return Ok(data);
    }

    [HttpPost("EditProfileImage")]
    public async Task<IActionResult> EditProfileImage(AdminEditProfileRequestModel requestModel)
    {
        var data = await _blAdmin.EditProfileImage(requestModel);
        return Ok(data);
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(AdminChangePasswordRequestModel requestModel)
    {
        var data = await _blAdmin.ChangePassword(requestModel);
        return Ok(data);
    }

    [HttpPost("Export")]
    public async Task<IActionResult> Export(AdminExportRequestModel requestModel)
    {
        try
        {
            return requestModel.Format.ToLower() switch
            {
                "csv" => File(
                    await _exportService.ExportToCsv(requestModel.AdminList),
                    "text/csv",
                    "Admin_User.csv"),

                "xlsx" or "excel" => File(
                    await _exportService.ExportToExcel(requestModel.AdminList, "Admin User"),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Admin_User.xlsx"),

                "pdf" => File(
                    await _exportService.ExportToPdf(requestModel.AdminList, "Admin User"),
                    "application/pdf",
                    "Admin_User.pdf"),

                _ => BadRequest("Unsupported format. Use csv, xlsx, or pdf")
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }
}