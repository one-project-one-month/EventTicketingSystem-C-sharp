using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.BusinessEmail
{
    public class BusinessEmailExportRequestModel
    {
        public string Format {  get; set; }
        public List<BusinessEmailListModel> BusinessEmailList { get; set; }
    }
}
