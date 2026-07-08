namespace EventTicketingSystem.CSharp.Domain.Features.Home;

public class DA_Home
{
    private readonly AppDbContext _db;
    private readonly ILogger<DA_Home> _logger;

    public DA_Home(AppDbContext db, ILogger<DA_Home> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Result<HomeResponseModel>> GetHome()
    {
        try
        {
            var now = DateTime.Now;
            var topEvents = await (
                from evt in _db.TblEvents
                join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
                where evt.Deleteflag == false && venue.Deleteflag == false && evt.Isactive
                orderby evt.Startdate
                select new HomeEventModel
                {
                    Eventid = evt.Eventid,
                    Eventcode = evt.Eventcode,
                    Eventname = evt.Eventname,
                    Address = venue.Address,
                    Venueimage = SplitCsv(venue.Venueimage)
                }).Take(3).ToListAsync();

            var topVenues = await (
                from venue in _db.TblVenues
                join venueType in _db.TblVenuetypes on venue.Venuetypecode equals venueType.Venuetypecode
                where venue.Deleteflag == false && venueType.Deleteflag == false
                orderby venue.Createdat descending
                select new HomeVenueModel
                {
                    Venueid = venue.Venueid,
                    Venuecode = venue.Venuecode,
                    Venuename = venue.Venuename,
                    Venuetypename = venueType.Venuetypename,
                    Capacity = venue.Capacity,
                    Venueimage = SplitCsv(venue.Venueimage),
                    Address = venue.Address
                }).Take(3).ToListAsync();

            var totalTickets = await _db.TblTickets.CountAsync(x => x.Deleteflag == false);
            var soldTickets = await _db.TblTickets.CountAsync(x => x.Deleteflag == false && x.Isused);

            var model = new HomeResponseModel
            {
                TopThreeEvents = new HomeEventResponse { Events = topEvents },
                TopThreeVenues = new HomeVenueResponse { Venues = topVenues },
                HomeStatus = new HomeStatusModel
                {
                    CompletedEvents = await _db.TblEvents.CountAsync(x => x.Deleteflag == false && x.Enddate < now),
                    ActiveEvents = await _db.TblEvents.CountAsync(x => x.Deleteflag == false && x.Isactive && x.Enddate >= now),
                    TotalVenues = await _db.TblVenues.CountAsync(x => x.Deleteflag == false),
                    TicketsSoldPercentage = totalTickets == 0 ? 0 : (int)Math.Round(soldTickets * 100m / totalTickets)
                }
            };

            return Result<HomeResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<HomeResponseModel>.SystemError(ex.Message);
        }
    }

    private static List<string> SplitCsv(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
    }
}
