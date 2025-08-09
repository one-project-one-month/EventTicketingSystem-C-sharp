namespace EventTicketingSystem.CSharp.Domain.Models.Features.Transaction;

public class TransactionRequestModel
{
    public string EventCode { get; set; }

    public string FullName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public string TicketTypeCode { get; set; }

    public int TicketQuantity { get; set; }
}
