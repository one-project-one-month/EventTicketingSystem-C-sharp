namespace EventTicketingSystem.CSharp.Domain.Features.Transaction;

public class BL_Transaction
{
    private readonly DA_Transaction _daTransaction;

    public BL_Transaction(DA_Transaction daTransaction)
    {
        _daTransaction = daTransaction;
    }

    public async Task<Result<EventTicketTypeListResponseModel>> GetTicketTypeList(string eventCode)
    {
        return await _daTransaction.GetTicketTypeList(eventCode);
    }

    public async Task<Result<TransactionResponseModel>> ProcessTransaction(TransactionRequestModel requestModel)
    {
        return await _daTransaction.ProcessTransaction(requestModel);
    }

    public async Task<Result<TransactionHistoryListResponseModel>> GetTransactionHistoryList()
    {
        return await _daTransaction.PurchasedHistoryList();
    }

    public async Task<Result<TransactionHistoryDetailResponseModel>> GetTransactionHistoryDetail(string transactionCode)
    {
        return await _daTransaction.PurchasedHistoryDetail(transactionCode);
    }
}