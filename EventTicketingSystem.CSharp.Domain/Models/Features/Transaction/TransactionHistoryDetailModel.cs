namespace EventTicketingSystem.CSharp.Domain.Models.Features.Transaction;

public class TransactionHistoryDetailModel
{
    public string Email { get; set; } = string.Empty;

    public string EventName { get; set; } = string.Empty;

    public string EventCode { get; set; } = string.Empty;

    public string EventStatus { get; set; } = string.Empty;

    public string TicketTypeName { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }

    public string PaymentType { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; }

    public bool IsActive { get; set; }

    public string Qr { get; set; } = string.Empty;
}