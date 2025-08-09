using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.VerificationCode
{
    public class VCExportRequestModel
    {
        public string Format { get; set; } 
        public List<VCodeModel> VCodeList { get; set; }
    }
}
