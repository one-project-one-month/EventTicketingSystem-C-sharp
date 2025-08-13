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

    public List<string> Venueimage { get; set; }

    public static VenueListModelForUser FromTblVenue(TblVenue venue, TblVenuetype venuetype, string domainUrl)
    {
        var venueModel =  new VenueListModelForUser
        {
            VenueCode = venue.Venuecode,
            VenueTypeCode = venue.Venuetypecode,
            Venuetypename = venuetype.Venuetypename,
            VenueName = venue.Venuename,
            Capacity = venue.Capacity,
            Address = venue.Address,
            Venueimage = new List<string>()
        };

        if (!venue.Venueimage.IsNullOrEmpty())
        {
            venueModel.Venueimage = venue.Venueimage
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            foreach (var image in venueModel.Venueimage)
            {
                if (!image.IsNullOrEmpty())
                {
                    var baseUrl = domainUrl!.EndsWith("/") ? domainUrl : domainUrl + "/";
                    var imagePath = image.StartsWith("/") ? image.Substring(1) : image;

                    venueModel.Venueimage = new List<string> { $"{baseUrl}{imagePath}" };
                }
            }
        }

        return venueModel;
    }
}