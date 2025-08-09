using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Venue
{
    public class VenueExportRequestModle
    {
        public string Format { get; set; }
        public List<VenueTypeListModel> VenueList { get; set; }
    }
}
