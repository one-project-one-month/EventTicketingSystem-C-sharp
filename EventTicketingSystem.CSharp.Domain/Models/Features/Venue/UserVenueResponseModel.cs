namespace EventTicketingSystem.CSharp.Domain.Models.Features.Venue;

public class UserVenueListResponseModel
{
    public List<UserVenueListModel> VenueList { get; set; } = new();

    public int TotalRowCount { get; set; }

    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }
}

public class UserVenueListModel
{
    public string VenueCode { get; set; } = string.Empty;

    public string VenueTypeCode { get; set; } = string.Empty;

    public string Venuetypename { get; set; } = string.Empty;

    public string VenueName { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public string Address { get; set; } = string.Empty;

    public List<string> Venueimage { get; set; } = new();
}

public class UserVenueDetailResponseModel
{
    public UserVenueDetailModel Venue { get; set; } = new();
}

public class UserVenueDetailModel
{
    public string VenueCode { get; set; } = string.Empty;

    public string VenueTypeCode { get; set; } = string.Empty;

    public string VenueName { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Addons { get; set; } = new();

    public string Facilities { get; set; } = string.Empty;

    public List<string> VenueImage { get; set; } = new();
}
