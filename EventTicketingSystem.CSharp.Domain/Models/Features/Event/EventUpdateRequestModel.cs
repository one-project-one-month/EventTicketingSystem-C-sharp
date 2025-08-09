namespace EventTicketingSystem.CSharp.Domain.Models.Features.Event;

public class EventUpdateRequestModel
{
    public string EventCode { get; set; }

    public bool Isactive { get; set; }
    
    public string Eventstatus { get; set; }

}