using EventTicketingSystem.CSharp.Domain.Models.Features.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystem.CSharp.Domain.Features.Dashboard;

public class DA_Dashboard
{
    private readonly AppDbContext _db;
    private readonly ILogger<DA_Dashboard> _logger;

    public DA_Dashboard(AppDbContext db, ILogger<DA_Dashboard> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Result<DashboardResponseModel>> GetDashboardSummary()
    {
        var response = new Result<DashboardResponseModel>();
        try
        {
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            var firstDayOfThisMonth = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1), DateTimeKind.Unspecified);
            var firstDayOfPrevMonth = DateTime.SpecifyKind(firstDayOfThisMonth.AddMonths(-1), DateTimeKind.Unspecified);
            var lastDayOfPrevMonth = DateTime.SpecifyKind(firstDayOfThisMonth.AddDays(-1), DateTimeKind.Unspecified);

            // Event counts
            var totalEventThisMonth = await _db.TblEvents.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfThisMonth && x.Createdat <= now);
            var totalEventPrevMonth = await _db.TblEvents.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfPrevMonth && x.Createdat <= lastDayOfPrevMonth);
            var eventDiff = CalculatePercentageDiff(totalEventThisMonth, totalEventPrevMonth);
            var totalEvent = await _db.TblEvents.CountAsync(x => !x.Deleteflag);

            // Venue counts
            var totalVenueThisMonth = await _db.TblVenues.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfThisMonth && x.Createdat <= now);
            var totalVenuePrevMonth = await _db.TblVenues.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfPrevMonth && x.Createdat <= lastDayOfPrevMonth);
            var venueDiff = CalculatePercentageDiff(totalVenueThisMonth, totalVenuePrevMonth);
            var totalVenue = await _db.TblVenues.CountAsync(x => !x.Deleteflag);

            // Admin counts
            var totalAdminThisMonth = await _db.TblAdmins.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfThisMonth && x.Createdat <= now);
            var totalAdminPrevMonth = await _db.TblAdmins.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfPrevMonth && x.Createdat <= lastDayOfPrevMonth);
            var adminDiff = CalculatePercentageDiff(totalAdminThisMonth, totalAdminPrevMonth);
            var totalAdmin = await _db.TblAdmins.CountAsync(x => !x.Deleteflag);

            // BO count
            var totalBOThisMonth = await _db.TblBusinessowners.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfThisMonth && x.Createdat <= now);
            var totalBOPrevMonth = await _db.TblBusinessowners.CountAsync(x => !x.Deleteflag && x.Createdat >= firstDayOfPrevMonth && x.Createdat <= lastDayOfPrevMonth);
            var bODiff = CalculatePercentageDiff(totalBOThisMonth, totalBOPrevMonth);
            var totalBO = await _db.TblBusinessowners.CountAsync(x => !x.Deleteflag);

            // Ticket Type Count - Week
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);
            var ticketTypeWeek = await (
                from ttt in _db.TblTransactiontickets
                where !ttt.Deleteflag && ttt.Createdat >= startOfWeek && ttt.Createdat <= endOfWeek
                join t in _db.TblTickets.Where(x => !x.Deleteflag) on ttt.Ticketcode equals t.Ticketcode
                join tp in _db.TblTicketprices.Where(x => !x.Deleteflag) on t.Ticketpricecode equals tp.Ticketpricecode
                join tt in _db.TblTickettypes.Where(x => !x.Deleteflag) on tp.Tickettypecode equals tt.Tickettypecode
                group ttt by tt.Tickettypename into g
                orderby g.Count() descending
                select new TTCount
                {
                    Label = g.Key,
                    TotalCount = g.Count()
                }
            ).Take(3).ToListAsync();

            // Ticket Type Count - Month
            var ticketTypeMonth = await (
                from ttt in _db.TblTransactiontickets
                where !ttt.Deleteflag && ttt.Createdat >= firstDayOfThisMonth && ttt.Createdat <= now
                join t in _db.TblTickets.Where(x => !x.Deleteflag) on ttt.Ticketcode equals t.Ticketcode
                join tp in _db.TblTicketprices.Where(x => !x.Deleteflag) on t.Ticketpricecode equals tp.Ticketpricecode
                join tt in _db.TblTickettypes.Where(x => !x.Deleteflag) on tp.Tickettypecode equals tt.Tickettypecode
                group ttt by tt.Tickettypename into g
                orderby g.Count() descending
                select new TTCount
                {
                    Label = g.Key,
                    TotalCount = g.Count()
                }
            ).Take(3).ToListAsync();

            var ticketCounts = new List<DashboardTTCount>
            {
                new DashboardTTCount
                {
                    TicketCountPeriod = TTCountType.Week,
                    TTCounts = ticketTypeWeek
                },
                new DashboardTTCount
                {
                    TicketCountPeriod = TTCountType.Month,
                    TTCounts = ticketTypeMonth
                }
            };

            // Ticket Sales by month (successful transactions only, Jan-Dec current year)
            var currentYear = now.Year;
            var transactionTicketsByMonth = await (
                from tr in _db.TblTransactions
                where !tr.Deleteflag
                      && tr.Status.ToLower() == "success"
                      && tr.Transactiondate.Year == currentYear
                join ttt in _db.TblTransactiontickets.Where(x => !x.Deleteflag) on tr.Transactioncode equals ttt.Transactioncode
                group ttt by tr.Transactiondate.Month into g
                select new
                {
                    Month = g.Key,
                    TotalCount = g.Count()
                }
            ).ToListAsync();

            var ticketSales = Enumerable.Range(1, 12)
                .Select(m => new DashboardTicketSale
                {
                    Month = new DateTime(currentYear, m, 1).ToString("MMM"),
                    TotalCount = transactionTicketsByMonth.FirstOrDefault(x => x.Month == m)?.TotalCount ?? 0
                })
                .ToList();

            var data = new DashboardResponseModel
            {
                TotalEvent = new DashboardCount { TotalCount = totalEvent, Difference = eventDiff },
                TotalVenue = new DashboardCount { TotalCount = totalVenue, Difference = venueDiff },
                TotalAdmin = new DashboardCount { TotalCount = totalAdmin, Difference = adminDiff },
                TotalBO = new DashboardCount { TotalCount = totalBO, Difference = bODiff },
                TicketCounts = ticketCounts,
                TicketSales = ticketSales
            };

            response = Result<DashboardResponseModel>.Success(data, "Here is the dashboard data!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard summary");
            response = Result<DashboardResponseModel>.SystemError();
        }
        return response;
    }

    private int CalculatePercentageDiff(int current, int previous)
    {
        if (previous == 0)
        {
            if (current == 0) return 0;
            return 100; // 100% increase from 0 to something
        }
        return (int)Math.Round(((double)(current - previous) / Math.Abs(previous)) * 100);
    }
}