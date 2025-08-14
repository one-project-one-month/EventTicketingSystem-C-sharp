namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class HomeResponseModel
{
    public TopThreeUserEventResponseModel TopThreeEvents { get; set; }

    public HomeStatusResponseModel HomeStatus { get; set; }

    public TopThreeVenueResponseModel TopThreeVenues { get; set; }
}

public class HomeStatusResponseModel
{
    public int CompletedEvents { get; set; }

    public int ActiveEvents { get; set; }

    public int TotalVenues { get; set; }

    public int TicketsSoldPercentage { get; set; }
}