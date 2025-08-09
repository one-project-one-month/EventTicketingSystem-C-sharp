using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Event
{
    public class EventExportRequestModel
    {
        public string Format { get; set; }
        public List<EventListModel> EventList { get; set; }
    }
}
