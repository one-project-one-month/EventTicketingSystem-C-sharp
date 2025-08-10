namespace EventTicketingSystem.CSharp.Domain.Models.Features.Transaction;

public class TransactionHistoryListResponseModel
{
    public List<TransactionHistoryModel> TransactionList { get; set; } = new List<TransactionHistoryModel>();
}
