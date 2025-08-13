namespace EventTicketingSystem.CSharp.Domain.Models.Features.Event;

public class EventListResponseModel
{
    public List<EventListModel>? EventList { get; set; }
}

public class EventListModel
{
    
    public string? Eventcode { get; set; }

    public string? Eventname { get; set; }
    
    public string? Uniquename { get; set; }
    
    public string? Businessownername { get; set; }

    public bool? Isactive { get; set; }
    

    public static EventListModel FromTblEvent(TblEvent tblEvent)
    {
        return new EventListModel
        {
            Eventcode = tblEvent.Eventcode,
            Eventname = tblEvent.Eventname,
            Uniquename = tblEvent.Uniquename,
            Isactive = tblEvent.Isactive,
        };
    }
}