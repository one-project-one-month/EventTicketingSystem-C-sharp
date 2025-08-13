namespace EventTicketingSystem.CSharp.Domain.Models.Features.Venue;

public class VenueEditResponseModel
{
    public VenueEditModel? Venue { get; set; }
}

public class VenueEditModel
{
    public string VenueCode { get; set; }
    
    public string VenueTypeCode { get; set; }
    
    public string VenueName { get; set; }
    
    public int? Capacity { get; set; }
    
    public string? Address { get; set; }
    
    public string? Description { get; set; }
    
    public List<string>? Addons { get; set; }
    
    public string? Facilities { get; set; }

    public List<string> VenueImage { get; set; }

    public static VenueEditModel FromTblVenue(TblVenue venue, string domainUrl)
    {
        var venueModel = new VenueEditModel
        {
            VenueCode = venue.Venuecode,
            VenueTypeCode = venue.Venuetypecode,
            VenueName = venue.Venuename,
            Capacity = venue.Capacity,
            Address = venue.Address,
            Description = venue.Description,
            Addons = new List<string>(),
            Facilities = venue.Facilities,
            VenueImage = new List<string>()
        };
        
        if (!venue.Venueimage.IsNullOrEmpty())
        {
            venueModel.VenueImage = venue.Venueimage
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            foreach (var image in venueModel.VenueImage)
            {
                if (!image.IsNullOrEmpty())
                {
                    var baseUrl = domainUrl!.EndsWith("/") ? domainUrl : domainUrl + "/";
                    var imagePath = image.StartsWith("/") ? image.Substring(1) : image;

                    venueModel.VenueImage = new List<string> { $"{baseUrl}{imagePath}" };
                }
            }
        }
        
        if (!venue.Addons.IsNullOrEmpty())
        {
            venueModel.Addons = venue.Addons!
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }
        
        return venueModel;
    }
}