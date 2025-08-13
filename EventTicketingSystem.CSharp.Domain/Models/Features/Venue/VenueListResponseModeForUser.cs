namespace EventTicketingSystem.CSharp.Domain.Models.Features.Venue;

public class VenueListResponseModeForUser : PaginationModel
{
    public List<VenueListModelForUser>? VenueList { get; set; }
    public List<VenueListModelForUser>? Top3Venues { get; set; }
}

public class VenueListModelForUser
{
    public string VenueCode { get; set; }

    public string VenueTypeCode { get; set; }
    public string Venuetypename { get; set; }

    public string VenueName { get; set; }

    public int? Capacity { get; set; }

    public string Address { get; set; }
    public string Venueimage { get; set; }

    public static VenueListModelForUser FromTblVenue(TblVenue venue, TblVenuetype venuetype)
    {
        return new VenueListModelForUser
        {
            VenueCode = venue.Venuecode,
            VenueTypeCode = venue.Venuetypecode,
            Venuetypename = venuetype.Venuetypename,
            VenueName = venue.Venuename,
            Capacity = venue.Capacity,
            Address = venue.Address,
            Venueimage = venue.Venueimage
        };
    }
}