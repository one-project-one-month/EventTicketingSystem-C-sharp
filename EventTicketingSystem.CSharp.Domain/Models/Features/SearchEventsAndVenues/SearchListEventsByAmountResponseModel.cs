namespace EventTicketingSystem.CSharp.Domain.Models.Features.SearchEventsAndVenues;

public class SearchListEventsByAmountResponseModel
{
    public List<SearchEventByAmountResponseModel> Events { get; set; } = new List<SearchEventByAmountResponseModel>();
}

public class SearchEventByAmountResponseModel
{
    public string? Eventid { get; set; }

    public string? Eventcode { get; set; }

    public string? Eventname { get; set; }

    public string? Categorycode { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public string? Eventimage { get; set; }

    public List<string> Venueimage { get; set; } = new();

    public bool? Isactive { get; set; }

    public string? Eventstatus { get; set; }

    public string? Businessownercode { get; set; }

    public int? Totalticketquantity { get; set; }

    public int? Soldoutcount { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createdat { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

    //public List<SearchTicketPriceResponseModel> TotalTicketPrice { get; set; }
}

public class SearchTicketPriceResponseModel
{
    public string Ticketpriceid { get; set; } = null!;

    public string Ticketpricecode { get; set; } = null!;

    public string Eventcode { get; set; } = null!;

    public string Tickettypecode { get; set; } = null!;

    public decimal Ticketprice { get; set; }

    public int Ticketquantity { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

}
