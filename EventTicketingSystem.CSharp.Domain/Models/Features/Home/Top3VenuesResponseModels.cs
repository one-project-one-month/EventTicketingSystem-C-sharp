using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Home;

public class Top3VenuesResponseModels
{
    public List<VenuesResponseModels> Venues { get; set; }
}

public class VenuesResponseModels
{
    public string Venueid { get; set; } = null!;
    public string Venuename { get; set; } = null!;
    public string Venuetypename { get; set; } = null!;
    public int Capacity { get; set; }
    public string Address { get; set; } = null!;

}
