using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.BusinessOwner
{
    public class BusinessOwnerExportRequestModel
    {
        public string Format { get; set; }
        public List<BusinessOwnerListModel> BusinessOwnerList { get; set; }
    }
}
