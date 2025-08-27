using BoldReports.Web.ReportViewer;
using EventTicketingSystem.CSharp.Domain.Models.Features.Report;
using EventTicketingSystem.CSharp.Domain.Services.ReportContracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventTicketingSystem.CSharp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportFactory _reportFactory;

        public ReportController(ReportFactory reportFactory)
        {
            _reportFactory = reportFactory;
        }

        [HttpPost("{reportName}")]
        public async Task<IActionResult> Export(string reportName, [FromQuery] string? exportFormat, [FromBody] JsonElement requestData)
        {
            var reportService = _reportFactory.GetReportService(reportName);
            object typedRequest = reportName switch
            {
                "EventReport" => JsonSerializer.Deserialize<EventReportRequest>(requestData.GetRawText())!,
                _ => throw new ArgumentException($"Unsupported report: {reportName}")
            };
            var result = await reportService.GenerateAsync(typedRequest, exportFormat);

            if (result.IsForView)
                return Ok(new { Message = "Report ready for viewer." });

            return File(result.FileBytes!, result.MimeType, result.FileName);
        }
    }
}
