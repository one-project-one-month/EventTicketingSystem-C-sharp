namespace EventTicketingSystem.CSharp.Domain.Features.Transaction;

public class BL_Transaction
{
    private readonly DA_Transaction _dataAccess;

    public BL_Transaction(DA_Transaction dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Result<ProcessTransactionResponseModel>> ProcessTransaction(ProcessTransactionRequestModel requestModel)
    {
        return await _dataAccess.ProcessTransaction(requestModel);
    }

    public async Task<Result<TransactionHistoryListResponseModel>> GetTransactionHistoryList()
    {
        return await _dataAccess.GetTransactionHistoryList();
    }

    public async Task<Result<TransactionDetailResponseModel>> GetTransactionHistoryDetail(string transactionCode)
    {
        return await _dataAccess.GetTransactionHistoryDetail(transactionCode);
    }
}
