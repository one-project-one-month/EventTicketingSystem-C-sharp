using BoldReports.Web.ReportViewer;
using EventTicketingSystem.CSharp.Domain.Models.Features.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Services.ReportContracts
{
    public interface IReportService
    {
        string ReportName { get; }
        Dictionary<string, object> GetDataSources(object requestData);
        Dictionary<string, string>? GetParameters(object requestData);
        Task<ReportResultModel> GenerateAsync(object requestData, string? exportFormat = null);
    }
}
