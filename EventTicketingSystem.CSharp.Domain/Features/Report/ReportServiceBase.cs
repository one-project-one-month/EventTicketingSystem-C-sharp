using BoldReports.Web;
using BoldReports.Writer;
using EventTicketingSystem.CSharp.Domain.Models.Features.Report;
using EventTicketingSystem.CSharp.Domain.Services.ReportContracts;
using System.Reflection;

namespace EventTicketingSystem.CSharp.Domain.Features.Report
{
    public abstract class ReportServiceBase : IReportService
    {
        public abstract string ReportName { get; }
        public abstract Dictionary<string, object> GetDataSources(object requestData);
        public virtual Dictionary<string, object>? GetParameters(object requestData) => null;

        protected Stream GetRdlcStream()
        {
            var assembly = typeof(ReportServiceBase).Assembly;
            string resourceName = "EventTicketingSystem.CSharp.Domain.Features.Report." + ReportName + ".rdlc";

            return assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"RDLC '{ReportName}' not found as embedded resource.");
        }

        public async Task<ReportResultModel> GenerateAsync(object requestData, string? exportFormat = null)
        {
            var dataSources = GetDataSources(requestData);

            using var rdlcStream = GetRdlcStream();
            using var report = new ReportWriter();
            report.LoadReport(rdlcStream);
            report.ReportProcessingMode = ProcessingMode.Local;

            foreach (var ds in dataSources)
            {
                report.DataSources.Add(new ReportDataSource(ds.Key, ds.Value));
            }
            var reportParams = ((IReportService)this).GetParameters(requestData);
            List<ReportParameter> boldReportParameters = new List<ReportParameter>();
            foreach (var kvp in reportParams)
            {
                boldReportParameters.Add(new ReportParameter
                {
                    Name = kvp.Key,
                    Values = new List<string> { kvp.Value }
                });
            }

            if (boldReportParameters.Count > 0)
            {
                report.SetParameters(boldReportParameters);
            }

            if (string.IsNullOrEmpty(exportFormat))
            {
                return new ReportResultModel { IsForView = true };
            }

            WriterFormat format = exportFormat.ToLower() switch
            {
                "pdf" => WriterFormat.PDF,
                "excel" => WriterFormat.Excel,
                "word" => WriterFormat.Word,
                "html" => WriterFormat.HTML,
                "csv" => WriterFormat.CSV,
                _ => throw new ArgumentException("Unsupported export format.")
            };

            using var memoryStream = new MemoryStream();
            report.Save(memoryStream, format);
            memoryStream.Position = 0;

            return new ReportResultModel
            {
                IsForView = false,
                FileBytes = memoryStream.ToArray(),
                MimeType = GetMimeType(format),
                FileName = $"{ReportName}.{exportFormat.ToLower()}"
            };
        }

        private static string GetMimeType(WriterFormat format) => format switch
        {
            WriterFormat.PDF => "application/pdf",
            WriterFormat.Excel => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            WriterFormat.Word => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            WriterFormat.HTML => "text/html",
            WriterFormat.CSV => "text/csv",
            _ => "application/octet-stream"
        };

        Dictionary<string, string>? IReportService.GetParameters(object requestData)
        {
            if (requestData == null)
                return new Dictionary<string, string>();

            var parameters = new Dictionary<string, string>();
            var props = requestData.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(requestData);
                parameters[prop.Name] = value?.ToString() ?? string.Empty;
            }

            return parameters;
        }
    }
}
