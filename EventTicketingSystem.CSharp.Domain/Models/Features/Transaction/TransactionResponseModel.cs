namespace EventTicketingSystem.CSharp.Domain.Models.Features.Transaction;

public class ProcessTransactionRequestModel
{
    public string EventCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string TicketTypeCode { get; set; } = string.Empty;

    public int TicketQuantity { get; set; }
}

public class ProcessTransactionResponseModel
{
    public ProcessTransactionRequestModel Response { get; set; } = new();
}

public class TransactionHistoryListResponseModel
{
    public List<TransactionHistoryModel> TransactionList { get; set; } = new();
}

public class TransactionHistoryModel
{
    public string TransactionCode { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; }

    public string EventName { get; set; } = string.Empty;

    public string TicketTypeName { get; set; } = string.Empty;
}

public class TransactionDetailResponseModel
{
    public TransactionDetailModel TransactionDetail { get; set; } = new();
}

public class TransactionDetailModel
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

    public int Qty { get; set; }
}
