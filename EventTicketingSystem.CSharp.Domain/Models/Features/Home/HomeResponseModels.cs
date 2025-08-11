using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class HomeResponseModels
{
    public Top3EventsResponseModels TopThreeEvents { get; set; }
    public HomeStatusResponseModels HomeStatus { get; set; }
    public Top3VenuesResponseModels TopThreeVenues { get; set; }
}

public class HomeStatusResponseModels
{
    public int CompletedEvents { get; set; }
    public int ActiveEvents { get; set; }
    public int TotalVenues { get; set; }
    public int TicketsSoldPercentage { get; set; }
}
