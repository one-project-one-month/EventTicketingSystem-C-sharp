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
            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfPrevMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPrevMonth = firstDayOfThisMonth.AddDays(-1);

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
            
            // For This week Best Selling - top 3 ticket types by tickets created this week
            
            var mondayOffset = ((int)now.DayOfWeek + 6) % 7; 
            var startOfWeek = now.Date.AddDays(-mondayOffset);
            var nextWeekStart  = startOfWeek.AddDays(7);
            
            var ticketTypeWeek = await (
                from tt in _db.TblTickettypes.Where(tt => !tt.Deleteflag)
                join tp in _db.TblTicketprices.Where(x => !x.Deleteflag) 
                    on tt.Tickettypecode equals tp.Tickettypecode
                join t in _db.TblTickets.Where(x => 
                    !x.Deleteflag && 
                    x.Createdat >= startOfWeek && x.Createdat < nextWeekStart)
                    on tp.Ticketpricecode equals t.Ticketpricecode
                group t by tt.Tickettypename into g
                select new { Label = g.Key, TotalCount = g.Count() }
            )
            .OrderByDescending(x => x.TotalCount)
            .Take(3)
            .Select(x => new TTCount { Label = x.Label, TotalCount = x.TotalCount })
            .ToListAsync();
            
            
            // For This month Best Selling - top 3 ticket types by tickets created this month
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            var ticketTypeMonth = await (
                    from tt in _db.TblTickettypes.Where(tt => !tt.Deleteflag)
                    join tp in _db.TblTicketprices.Where(x => !x.Deleteflag)
                        on tt.Tickettypecode equals tp.Tickettypecode
                    join t in _db.TblTickets.Where(x =>
                            !x.Deleteflag &&
                            x.Createdat >= monthStart &&
                            x.Createdat < nextMonthStart)
                        on tp.Ticketpricecode equals t.Ticketpricecode
                    group t by tt.Tickettypename
                    into g
                    select new { Label = g.Key, TotalCount = g.Count() }
                )
                .OrderByDescending(x => x.TotalCount)
                .Take(3)
                .Select(x => new TTCount { Label = x.Label, TotalCount = x.TotalCount })
                .ToListAsync();
               
            var ticketCounts = new List<DashboardTTCount>
            {
                new DashboardTTCount
                {
                    TicketCountPeriod = TicketCountPeriod.Week.ToString(),
                    TTCounts = ticketTypeWeek
                },
                new DashboardTTCount
                {
                    TicketCountPeriod = TicketCountPeriod.Month.ToString(),
                    TTCounts = ticketTypeMonth
                }
            };
             
            // Ticket Sales by month (successful transactions only, Jan-Dec current year)
            var currentYear = now.Year;
            var salesByMonth = await (
                from tr in _db.TblTransactions
                where !tr.Deleteflag
                      && tr.Status == EnumTransactionStatus.Success.ToString()
                      && tr.Transactiondate.Year == currentYear
                group tr by tr.Transactiondate.Month into g
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
                    TotalCount = salesByMonth.FirstOrDefault(x => x.Month == m)?.TotalCount ?? 0
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