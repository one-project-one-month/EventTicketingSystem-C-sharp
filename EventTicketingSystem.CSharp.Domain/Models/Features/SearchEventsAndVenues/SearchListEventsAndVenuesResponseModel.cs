namespace EventTicketingSystem.CSharp.Domain.Models.Features.SearchEventsAndVenues;

public class SearchListEventsAndVenuesResponseModel
{
    public List<SearchEventResponseModel> Events { get; set; } = new List<SearchEventResponseModel>();

    public List<SearchVenuesResponseModel> Venues { get; set; } = new List<SearchVenuesResponseModel>();
}

public class SearchEventResponseModel
{
    public string? Eventid { get; set; }

    public string? Eventcode { get; set; }

    public string? Eventname { get; set; }

    public string? Categorycode { get; set; }

    //public string? Description { get; set; }

    public string? Address { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

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
}

public class SearchVenuesResponseModel
{
    public string? Venueid { get; set; }

    public string? Venuecode { get; set; }

    public string? Venuename { get; set; }

    //public string? Venuedetailcode { get; set; }

    public string? Venuetypecode { get; set; }

    public string? Venuetypename { get; set; }

    public string? Venuedescription { get; set; }

    public string? Venueaddress { get; set; }

    public string? Address { get; set; }

    public int? Venuecapacity { get; set; }

    public int? Capacity { get; set; }

    public List<string> Venueimage { get; set; } = new();

    public string? Venuefacilities { get; set; }

    public string? Venueaddons { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createdat { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }
}
