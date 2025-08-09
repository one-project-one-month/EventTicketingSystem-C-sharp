using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.EventCategory
{
    public class EventCategoryExportRequestModel
    {
        public string Format {  get; set; }
        public List<EventCategoryListModel> EventCateogryList { get; set; }
    }
}
