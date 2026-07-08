namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class HomeResponseModel
{
    public HomeEventResponse TopThreeEvents { get; set; } = new();

    public HomeStatusModel HomeStatus { get; set; } = new();

    public HomeVenueResponse TopThreeVenues { get; set; } = new();
}

public class HomeEventResponse
{
    public List<HomeEventModel> Events { get; set; } = new();
}

public class HomeVenueResponse
{
    public List<HomeVenueModel> Venues { get; set; } = new();
}

public class HomeEventModel
{
    public string Eventid { get; set; } = string.Empty;

    public string Eventcode { get; set; } = string.Empty;

    public string Eventname { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public List<string> Venueimage { get; set; } = new();
}

public class HomeVenueModel
{
    public string Venueid { get; set; } = string.Empty;

    public string Venuecode { get; set; } = string.Empty;

    public string Venuename { get; set; } = string.Empty;

    public string Venuetypename { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public List<string> Venueimage { get; set; } = new();

    public string Address { get; set; } = string.Empty;
}

public class HomeStatusModel
{
    public int CompletedEvents { get; set; }

    public int ActiveEvents { get; set; }

    public int TotalVenues { get; set; }

    public int TicketsSoldPercentage { get; set; }
}
