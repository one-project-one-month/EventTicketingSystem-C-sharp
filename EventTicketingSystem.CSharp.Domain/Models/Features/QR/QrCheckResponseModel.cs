namespace EventTicketingSystem.CSharp.Domain.Models.Features.QR;

public class QrCheckResponseModel
{
    public string EventName { get; set; }

    public string EventCode { get; set; }

    public string EventDate { get; set; }

    public string EventTimeFrom { get; set; }

    public string EventTimeTo { get; set; }

    public string GateOpenTime { get; set; }

    public string TicketCode { get; set; }

    public string TicketPrice { get; set; }

    public string TicketType { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Location { get; set; }

    public string Address { get; set; }
}