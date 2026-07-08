namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Transaction")]
[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly BL_Transaction _blTransaction;

    public TransactionController(BL_Transaction blTransaction)
    {
        _blTransaction = blTransaction;
    }

    [HttpPost("ProcessTransaction")]
    [AllowAnonymous]
    public async Task<IActionResult> ProcessTransaction(ProcessTransactionRequestModel requestModel)
    {
        var data = await _blTransaction.ProcessTransaction(requestModel);
        return Ok(data);
    }

    [HttpGet("GetTransactionHistoryList")]
    [Authorize]
    public async Task<IActionResult> GetTransactionHistoryList()
    {
        var data = await _blTransaction.GetTransactionHistoryList();
        return Ok(data);
    }

    [HttpGet("GetTransactionHistoryDetail/{transactionCode}")]
    [Authorize]
    public async Task<IActionResult> GetTransactionHistoryDetail(string transactionCode)
    {
        var data = await _blTransaction.GetTransactionHistoryDetail(transactionCode);
        return Ok(data);
    }
}
