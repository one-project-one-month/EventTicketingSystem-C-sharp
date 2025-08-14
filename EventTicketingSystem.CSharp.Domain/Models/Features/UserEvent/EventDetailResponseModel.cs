namespace EventTicketingSystem.CSharp.Domain.Models.Features.UserEvent;

public class EventDetailResponseModel
{
    public string? Eventcode { get; set; }

    public string? Eventname { get; set; }

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public string? Venuename { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public int Capacity { get; set; }

    public string? Facilities { get; set; }

    public string? Addons { get; set; }

    public List<string> Venueimage { get; set; }

    public List<TicketTypeDetail> TicketTypes { get; set; }

    public static EventDetailResponseModel FromTbl(TblEvent tblEvent, TblVenue tblVenue, string userDomainUrl)
    {
        var eventModel = new EventDetailResponseModel
        {
            Eventcode = tblEvent.Eventcode,
            Eventname = tblEvent.Eventname,
            Startdate = tblEvent.Startdate,
            Enddate = tblEvent.Enddate,
            Venuename = tblVenue.Venuename,
            Description = tblVenue.Description,
            Address = tblVenue.Address,
            Capacity = tblVenue.Capacity,
            Facilities = tblVenue.Facilities,
            Addons = tblVenue.Addons,
            Venueimage = new List<string>(),
        };

        if (!tblVenue.Venueimage.IsNullOrEmpty())
        {
            eventModel.Venueimage = tblVenue.Venueimage
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            foreach (var image in eventModel.Venueimage)
            {
                if (!image.IsNullOrEmpty())
                {
                    var baseUrl = userDomainUrl!.EndsWith("/") ? userDomainUrl : userDomainUrl + "/";
                    var imagePath = image.StartsWith("/") ? image.Substring(1) : image;

                    eventModel.Venueimage = new List<string> { $"{baseUrl}{imagePath}" };
                }
            }
        }

        return eventModel;
    }
}

public class TicketTypeDetail
{
    public string? Tickettypecode { get; set; }

    public string? Tickettypename { get; set; }

    public decimal Ticketprice { get; set; }
}