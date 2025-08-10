namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("QR Code")]
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class QrCodeController : ControllerBase
{
    private readonly BL_QrCode _bl_QrCode;

    public QrCodeController(BL_QrCode bl_QrCode)
    {
        _bl_QrCode = bl_QrCode;
    }

    [HttpPost("Generate")]
    public async Task<IActionResult> Generate([FromBody] QrGenerateRequestModel requestModel)
    {
        var data = await _bl_QrCode.Generate(requestModel);
        return Ok(data);
    }

    [HttpGet("{qrString}")]
    public async Task<IActionResult> Check(string qrString)
    {
        if (string.IsNullOrEmpty(qrString))
        {
            return BadRequest("QR string cannot be null or empty");
        }

        var result = await _bl_QrCode.Check(qrString);
        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        return Ok(result);
    }
}