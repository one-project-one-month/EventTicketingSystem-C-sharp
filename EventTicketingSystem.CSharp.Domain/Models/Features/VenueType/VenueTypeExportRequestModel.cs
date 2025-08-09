using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.VenueType
{
    public class VenueTypeExportRequestModel
    {
        public string Format {  get; set; }
        public List<VenueTypeListModel> VenueTypeList { get; set;}
    }
}
