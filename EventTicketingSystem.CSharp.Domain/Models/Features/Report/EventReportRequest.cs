using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Models.Features.Report
{
    public class EventReportRequest
    {
        [JsonPropertyName("eventCode")]
        public string eventCode { get; set; } = string.Empty;
    }
}
