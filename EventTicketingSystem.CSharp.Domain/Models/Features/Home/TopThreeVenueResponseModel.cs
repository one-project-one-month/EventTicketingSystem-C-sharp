namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class TopThreeVenueResponseModel
{
    public List<UserVenueResponseModel> Venues { get; set; }
}

public class UserVenueResponseModel
{
    public string Venueid { get; set; } = null!;

    public string Venuecode { get; set; } = null!;

    public string Venuename { get; set; } = null!;

    public string Venuetypename { get; set; } = null!;

    public int Capacity { get; set; }

    public List<string> Venueimage { get; set; } = null!;

    public string Address { get; set; } = null!;
}