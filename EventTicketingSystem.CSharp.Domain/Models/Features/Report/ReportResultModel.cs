using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Report
{
    public class ReportResultModel
    {
        public bool IsForView { get; set; } 
        public byte[]? FileBytes { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
