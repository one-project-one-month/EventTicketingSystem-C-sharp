using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class Top3EventsResponseModels
{
    public List<EventsResponseModels> Events { get; set; }
}

public class EventsResponseModels
{
    public string Eventid { get; set; }
    public string Eventcode { get; set; }
    public string Eventname { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Venueimage { get; set; } = null!;
}
