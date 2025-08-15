namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Transaction")]
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class TransactionController : ControllerBase
{
    private readonly BL_Transaction _blTransaction;

    public TransactionController(BL_Transaction blTransaction)
    {
        _blTransaction = blTransaction;
    }

    [HttpGet("GetTicketList/{eventCode}")]
    [Authorize]
    public async Task<IActionResult> GetTicketTypeList(string eventCode)
    {
        return Ok(await _blTransaction.GetTicketTypeList(eventCode));
    }

    [HttpGet("GetTransactionHistoryList")]
    [Authorize]
    public async Task<IActionResult> GetTransactionHistoryList()
    {
        return Ok(await _blTransaction.GetTransactionHistoryList());
    }

    [HttpGet("GetTransactionHistoryDetail/{transactionCode}")]
    [Authorize]
    public async Task<IActionResult> GetTransactionHistoryDetail(string transactionCode)
    {
        return Ok(await _blTransaction.GetTransactionHistoryDetail(transactionCode));
    }

    [HttpPost("ProcessTransaction")]
    [AllowAnonymous]
    public async Task<IActionResult> ProcessTransaction([FromBody] TransactionRequestModel requestModel)
    {
        return Ok(await _blTransaction.ProcessTransaction(requestModel));
    }
}