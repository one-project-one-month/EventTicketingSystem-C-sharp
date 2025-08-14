namespace EventTicketingSystem.CSharp.Domain.Features.Home;

public class DA_Home
{
    private readonly AppDbContext _db;
    private readonly ILogger<DA_Admin> _logger;
    private readonly IConfiguration _configuration;

    public DA_Home(ILogger<DA_Admin> logger,
                    AppDbContext db,
                    IConfiguration configuration)
    {
        _logger = logger;
        _db = db;
        _configuration = configuration;
    }

    public async Task<Result<HomeResponseModel>> Home()
    {
        try
        {
            var EventResult = await (from e in _db.TblEvents
                                     join v in _db.TblVenues
                                         on e.Venuecode equals v.Venuecode
                                     where e.Deleteflag == false
                                     orderby e.Soldoutcount descending
                                     select new
                                     {
                                         e.Eventid,
                                         e.Eventcode,
                                         e.Eventname,
                                         v.Address,
                                         v.Venueimage,
                                     }
                                    )
                                    .Take(3)
                                    .ToListAsync();


            var EventResultCount = EventResult.Count();
            if (EventResultCount == 0)
            {
                return Result<HomeResponseModel>.NotFoundError("No events found.");
            }

            var domainUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EnumDirectory.wwwroot.ToString());
            
            var top3Events = new TopThreeUserEventResponseModel
            {
                Events = EventResult.Select(e => new HomeUserEventResponseModel
                {
                    Eventid = e.Eventid,
                    Eventcode = e.Eventcode,
                    Eventname = e.Eventname,
                    Address = e.Address,
                    Venueimage = ProcessVenueImages(e.Venueimage, domainUrl!)!,
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
                            v.Venuecode,
                            v.Venuename,
                            v.Capacity,
                            v.Venueimage,
                            v.Address,
                            vt.Venuetypename,
                        }
                    )
                    .ToListAsync();

            var VenuesResultCount = VenuesResult.Count();
            if (VenuesResultCount == 0)
            {
                return Result<HomeResponseModel>.NotFoundError("No Venues found.");
            }

            var Top3Venues = new TopThreeVenueResponseModel
            {
                Venues = VenuesResult.Select(v => new UserVenueResponseModel
                {
                    Venueid = v.Venueid,
                    Venuecode = v.Venuecode,
                    Venuename = v.Venuename,
                    Capacity = v.Capacity,
                    Venueimage = ProcessVenueImages(v.Venueimage, domainUrl!)!,
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

            var Top3 = new HomeResponseModel()
            {
                TopThreeEvents = top3Events,
                HomeStatus = new HomeStatusResponseModel
                {
                    CompletedEvents = CompletedEventsCount,
                    ActiveEvents = ActiveEventsCount,
                    TotalVenues = TotalVenuesCount,
                    TicketsSoldPercentage = ticketSoldoutPercentage
                },
                TopThreeVenues = Top3Venues

            };

            return Result<HomeResponseModel>.Success(Top3);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<HomeResponseModel>.SystemError(ex.Message);
        }
    }

    private List<string>? ProcessVenueImages(string? venueImages, string domainUrl)
    {
        if (string.IsNullOrWhiteSpace(venueImages))
        {
            return null!;
        }

        var baseUrl = domainUrl.TrimEnd('/') + "/";

        return venueImages
            .Split([','], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(image =>
            {
                var normalizedPath = image.TrimStart('/', '\\');
                return $"{baseUrl}{normalizedPath}";
            })
            .ToList();
    }
}