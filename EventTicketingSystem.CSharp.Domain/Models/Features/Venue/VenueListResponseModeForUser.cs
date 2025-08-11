namespace EventTicketingSystem.CSharp.Domain.Models.Features.Venue;

public class VenueListResponseModeForUser : PaginationModel
{
    public List<VenueListModelForUser>? VenueList { get; set; }
}

public class VenueListModelForUser
{
    public string VenueCode { get; set; }

    public string VenueTypeCode { get; set; }

    public string VenueName { get; set; }

    public int? Capacity { get; set; }

    public string Address { get; set; }

    public static VenueListModelForUser FromTblVenue(TblVenue venue)
    {
        return new VenueListModelForUser
        {
            VenueCode = venue.Venuecode,
            VenueTypeCode = venue.Venuetypecode,
            VenueName = venue.Venuename,
            Capacity = venue.Capacity,
            Address = venue.Address
        };
    }
}