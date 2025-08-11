namespace EventTicketingSystem.CSharp.Domain.Models.Features.Transaction;

public class TransactionHistoryModel
{
    public string TransactionCode { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; }

    public string EventName { get; set; } = string.Empty;
    
    public string TicketTypeName { get; set; } = string.Empty;
}