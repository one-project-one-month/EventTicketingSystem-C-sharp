using EventTicketingSystem.CSharp.Domain.Features.Report;
using EventTicketingSystem.CSharp.Domain.Models.Features.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Services.ReportContracts
{
    public class ReportFactory 
    {
        private readonly AppDbContext _db;

        public ReportFactory(AppDbContext db)
        {
            _db = db;
        }

        private readonly Dictionary<string, Func<IReportService>> _reportMap;

        public ReportFactory(AppDbContext db, IServiceProvider services)
        {
            _db = db;

            _reportMap = new Dictionary<string, Func<IReportService>>(StringComparer.OrdinalIgnoreCase)
            {
                { "EventReport", () => new EventReportService(_db) }
                // Add more reports here later
            };
        }

        public IReportService GetReportService(string reportName)
        {
            if (!_reportMap.TryGetValue(reportName, out var service))
                throw new ArgumentException($"Report '{reportName}' not found.");
            return service();
        }
    }
}
