namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class TopThreeUserEventResponseModel
{
    public List<HomeUserEventResponseModel> Events { get; set; }
}

public class HomeUserEventResponseModel
{
    public string Eventid { get; set; }

    public string Eventcode { get; set; }

    public string Eventname { get; set; } = null!;

    public string Address { get; set; } = null!;

    public List<string> Venueimage { get; set; } = null!;
}