using DocumentFormat.OpenXml.Drawing;
using EventTicketingSystem.CSharp.Domain.Models.Features.Home;

namespace EventTicketingSystem.CSharp.Domain.Features.Home;

public class DA_Home : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly ILogger<DA_Admin> _logger;
    private readonly CommonService _commonService;
    private string specialCharacters = @"!@#$%^&*(),.?""{}|<>";
    private int minLength = 8;
    private int maxLength = 20;

    public DA_Home(IHttpContextAccessor httpContextAccessor,
                    ILogger<DA_Admin> logger,
                    AppDbContext db,
                    CommonService commonService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
    }

    public async Task<Result<HomeResponseModels>> Home()
    {
        try
        {

            var EventResult = await (
                    from e in _db.TblEvents
                    join v in _db.TblVenues on e.Venuecode equals v.Venuecode
                    where e.Deleteflag == false
                    orderby e.Soldoutcount descending
                    select new
                    {
                        e.Eventid,
                        e.Eventname,
                        Address = v.Address
                    }
                )
                .Take(3)
                .ToListAsync();


            var EventResultCount = EventResult.Count();
            if (EventResultCount == 0)
            {
                return Result<HomeResponseModels>.NotFoundError("No events found.");
            }

            var top3Events = new Top3EventsResponseModels
            {
                Events = EventResult.Select(e => new EventsResponseModels
                {
                    Eventid = e.Eventid,
                    Eventname = e.Eventname,
                    Address = e.Address
                }).ToList()
            };

            var top3VenueCodes = await _db.TblEvents
                .Where(e => e.Deleteflag == false)
                .GroupBy(e => e.Venuecode)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToListAsync();

            var VenuesResult = await _db.TblVenues
                    .Where(v => top3VenueCodes.Contains(v.Venuecode) && v.Deleteflag == false)
                    .Join(
                        _db.TblVenuetypes,
                        v => v.Venuetypecode,
                        vt => vt.Venuetypecode,
                        (v, vt) => new
                        {
                            v.Venueid,
                            v.Venuename,
                            v.Capacity,
                            v.Address,
                            Venuetypename = vt.Venuetypename
                        }
                    )
                    .ToListAsync();

            var VenuesResultCount = VenuesResult.Count();
            if (VenuesResultCount == 0)
            {
                return Result<HomeResponseModels>.NotFoundError("No Venues found.");
            }

            var Top3Venues = new Top3VenuesResponseModels
            {
                Venues = VenuesResult.Select(v => new VenuesResponseModels
                {
                    Venueid = v.Venueid,
                    Venuename = v.Venuename,
                    Capacity = v.Capacity,
                    Address = v.Address,
                    Venuetypename = v.Venuetypename
                }).ToList()
            };

            var CompletedEventsResult = await _db.TblEvents
                                .Where(e => e.Deleteflag == false && e.Isactive == false)
                                .ToListAsync();

            var CompletedEventsCount = CompletedEventsResult.Count();

            var ActiveEventsResult = await _db.TblEvents
                                .Where(e => e.Deleteflag == false && e.Isactive == true)
                                .ToListAsync();

            var ActiveEventsCount = ActiveEventsResult.Count();

            var TotalVenues = await _db.TblVenues
                                .Where(x => x.Deleteflag == false)
                                .ToListAsync();

            var TotalVenuesCount = TotalVenues.Count();

            var ticketSoldoutSum = await _db.TblEvents
                .Where(e => e.Deleteflag == false && e.Isactive == true)
                .SumAsync(e => e.Soldoutcount);

            var Totalticketquantity = await _db.TblEvents
                .Where(e => e.Deleteflag == false && e.Isactive == true)
                .SumAsync(e => e.Totalticketquantity);

            var ticketSoldoutPercentage = Totalticketquantity > 0
                ? (ticketSoldoutSum * 100) / Totalticketquantity
                : 0;

            var Top3 = new HomeResponseModels()
            {
                TopThreeEvents = top3Events,
                HomeStatus = new HomeStatusResponseModels
                {
                    CompletedEvents = CompletedEventsCount,
                    ActiveEvents = ActiveEventsCount,
                    TotalVenues = TotalVenuesCount,
                    TicketsSoldPercentage = ticketSoldoutPercentage
                },
                TopThreeVenues = Top3Venues
                
            };

            return Result<HomeResponseModels>.Success(Top3);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<HomeResponseModels>.SystemError(ex.Message);
        }
    }
}
