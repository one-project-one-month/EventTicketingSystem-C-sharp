namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Ticket")]
[Route("api/[controller]")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly BL_Ticket _blTicket;

    public TicketController(BL_Ticket blTicket)
    {
        _blTicket = blTicket;
    }

    [HttpGet("List/{pageNo}")]
    public async Task<IActionResult> GetList(int pageNo)
    {
        var result = await _blTicket.List(pageNo);

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        return Ok(result.Data);
    }

    [HttpGet("Edit/{ticketCode}")]
    public async Task<IActionResult> GetTicketByCode(string ticketCode)
    {
        var result = await _blTicket.GetTicketByCode(ticketCode);
        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        return Ok(result.Data);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TicketCreateRequestModel requestModel)
    {
        if (requestModel == null)
        {
            return BadRequest("Request model cannot be null.");
        }
        var result = await _blTicket.CreateTicket(requestModel);

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        return Ok(result.Data);
    }

    [HttpPost("Update/{ticketCode},{isUsed}")]
    public async Task<IActionResult> UpdateTicket(string ticketCode, bool isUsed)
    {
        var result = await _blTicket.UpdateTicket(ticketCode, isUsed);

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        return Ok(result);
    }

    [HttpPost("Delete/{ticketCode}")]
    public async Task<IActionResult> DeleteByCode(string ticketCode)
    {
        var result = await _blTicket.DeleteByCode(ticketCode);

        if (result.IsError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        return Ok(result);
    }
}