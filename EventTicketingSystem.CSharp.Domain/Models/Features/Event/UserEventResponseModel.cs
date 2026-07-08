namespace EventTicketingSystem.CSharp.Domain.Models.Features.Event;

public class UserEventListResponseModel
{
    public List<UserEventListModel> EventList { get; set; } = new();

    public List<UserEventListModel> Top3Events { get; set; } = new();

    public int TotalRowCount { get; set; }

    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }
}

public class UserEventListModel
{
    public string Eventcode { get; set; } = string.Empty;

    public string Eventname { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public List<string> Venueimage { get; set; } = new();
}

public class UserEventDetailResponseModel
{
    public string Eventcode { get; set; } = string.Empty;

    public string Eventname { get; set; } = string.Empty;

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public string Venuename { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public string Facilities { get; set; } = string.Empty;

    public string Addons { get; set; } = string.Empty;

    public List<string> Venueimage { get; set; } = new();

    public List<UserEventTicketTypeModel> TicketTypes { get; set; } = new();
}

public class UserEventTicketTypeModel
{
    public string Tickettypecode { get; set; } = string.Empty;

    public string Tickettypename { get; set; } = string.Empty;

    public decimal Ticketprice { get; set; }
}

public class EventStatusOptionsResponseModel
{
    public List<EventStatusOptionModel> EventStatusOptions { get; set; } = new();
}

public class EventStatusOptionModel
{
    public string Value { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;
}
