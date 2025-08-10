namespace EventTicketingSystem.CSharp.Domain.Models.Features.Ticket;

public class TicketListResponseModel : PaginationModel
{
    public List<TicketListModel> Tickets { get; set; } = new List<TicketListModel>();
}

public class TicketListModel
{
    public string? Ticketid { get; set; }

    public string? Ticketcode { get; set; }

    public string? Ticketpricecode { get; set; }

    public bool? Isused { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createdat { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

    public bool? Deleteflag { get; set; }

    public string? Ticketpriceid { get; set; }

    public string? Eventcode { get; set; }

    public string? Tickettypecode { get; set; }

    public decimal? Ticketprice { get; set; }

    public int? Ticketquantity { get; set; }

    public string? Tickettypeid { get; set; }

    public string? Tickettypename { get; set; }
}