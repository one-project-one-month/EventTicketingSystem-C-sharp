namespace EventTicketingSystem.CSharp.Domain.Models.Features.QR;

public class QrGenerateModel
{
    public string TicketPriceCode { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public string TicketTypeCode { get; set; } = string.Empty;
    public string TicketTypeName { get; set; } = string.Empty;
    public string EventCode { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
