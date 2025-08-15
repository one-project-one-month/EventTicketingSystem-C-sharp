using EventTicketingSystem.CSharp.Database.AppDbContext;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.SearchEventsAndVenues;

public class SearchListEventsAndVenuesResponseModel
{
    public List<SearchEventResponseModel> Events { get; set; } = new List<SearchEventResponseModel>();

    public List<SearchVenuesResponseModel> Venues { get; set; } = new List<SearchVenuesResponseModel>();
}

public class SearchEventResponseModel
{
    public string Eventid { get; set; }
    public string Eventcode { get; set; }
    public string Eventname { get; set; }
    public string Address { get; set; }
    public List<string> Venueimage { get; set; }

    public static SearchEventResponseModel FromTbl (TblEvent ev, TblVenue ve)
    {
        var model = new SearchEventResponseModel
        {
            Eventid = ev.Eventid,
            Eventcode = ev.Eventcode,
            Eventname = ev.Eventname,
            Address = ve.Address,
            Venueimage = new List<string>()
        };

        if (!ve.Venueimage.IsNullOrEmpty())
        {
            model.Venueimage = ve.Venueimage
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }
        return model;
    }
}

public class SearchVenuesResponseModel
{
    public string Venueid { get; set; }
    public string Venuecode { get;set; }
    public string Venuename { get; set; }
    public string Venuetypename { get; set; }
    public int Capacity { get; set; }
    public List<string> Venueimage { get; set; }
    public string Address { get; set; }

    public static SearchVenuesResponseModel FromTbl(TblVenue ve, TblVenuetype vt)
    {
        var model = new SearchVenuesResponseModel
        {
            Venueid = ve.Venueid,
            Venuecode = ve.Venuecode,
            Venuename = ve.Venuename,
            Venuetypename = vt.Venuetypename,
            Capacity = ve.Capacity,
            Venueimage = new List<string>(),
            Address = ve.Address,
        };

        if (!ve.Venueimage.IsNullOrEmpty())
        {
            model.Venueimage = ve.Venueimage
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        return model;
    }
}