namespace EventTicketingSystem.CSharp.Domain.Features.SearchEventsAndVenues;

public class DA_SearchEventsAndVenues
{
    private readonly ILogger<DA_SearchEventsAndVenues> _logger;
    private readonly AppDbContext _db;

    public DA_SearchEventsAndVenues(ILogger<DA_SearchEventsAndVenues> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Result<SearchListEventsAndVenuesResponseModel>> SearchEventsAndVenues(string searchTerm)
    {
        try
        {
            var EventResult = await _db.TblEvents
                            .Where(
                                e => EF.Functions.ILike(e.Eventname!, "%" + searchTerm + "%") &&
                                e.Deleteflag == false
                            )
                            .Join(_db.TblVenues,
                            ev => ev.Venuecode,
                            ve => ve.Venuecode,
                            (ev, ve) => new {ev,ve})
                            .ToListAsync();

            var VenueResult = await _db.TblVenues
                            .Where(
                                v => EF.Functions.ILike(v.Venuename!, "%" + searchTerm + "%") &&
                                v.Deleteflag == false
                            )
                            .Join(_db.TblVenuetypes,
                            ve => ve.Venuetypecode,
                            vt =>  vt.Venuetypecode,
                            (ve, vt) => new {ve,vt})
                            .ToListAsync();

            var SearchResult = new SearchListEventsAndVenuesResponseModel
            {
                Events = EventResult.Select(x => SearchEventResponseModel.FromTbl(x.ev, x.ve)).ToList(),
                Venues = VenueResult.Select(x => SearchVenuesResponseModel.FromTbl(x.ve, x.vt)).ToList(),
            };

            var eventsData = SearchResult.Events.Count();
            var venuesData = SearchResult.Venues.Count();
            if (eventsData == 0 && venuesData == 0)
            {
                return Result<SearchListEventsAndVenuesResponseModel>.NotFoundError("No events or venues found matching the search term.");
            }

            return Result<SearchListEventsAndVenuesResponseModel>.Success(SearchResult);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<SearchListEventsAndVenuesResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<SearchListEventsResponseModel>> SearchEventsByDate(DateTime Startdate, DateTime Enddate)
    {
        try
        {
            //var nextDay = Enddate.AddDays(1);
            
            var startUtc = DateTime.SpecifyKind(Startdate.Date, DateTimeKind.Utc);
            var endUtc   = DateTime.SpecifyKind(Enddate.Date.AddDays(1), DateTimeKind.Utc);

            var EventResult = await _db.TblEvents
                .Where(e =>
                    e.Startdate >= startUtc &&
                    e.Enddate < endUtc &&
                    e.Deleteflag == false
                )
                .Join(_db.TblVenues,
                ev => ev.Venuecode,
                ve => ve.Venuecode,
                (ev, ve) => new { ev, ve})
                .ToListAsync();

            var SearchResult = new SearchListEventsResponseModel
            {
                Events = EventResult.Select(x => SearchEventResponseModel.FromTbl(x.ev, x.ve)).ToList(),
            };

            if (EventResult.Count == 0)
            {
                return Result<SearchListEventsResponseModel>.NotFoundError("No events found matching the search date.");
            }

            return Result<SearchListEventsResponseModel>.Success(SearchResult);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<SearchListEventsResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<SearchListEventsResponseModel>> SearchEventsByAmountAsync(decimal FromAmount, decimal ToAmount)
    {
        try
        {
            var events = (
                        from tp in _db.TblTicketprices
                        join tt in _db.TblTickettypes on tp.Tickettypecode equals tt.Tickettypecode
                        join e in _db.TblEvents on tt.Eventcode equals e.Eventcode
                        join ve in _db.TblVenues on e.Venuecode equals ve.Venuecode
                        where tp.Ticketprice >= FromAmount
                              && tp.Ticketprice <= ToAmount
                              && tp.Deleteflag == false
                              && tt.Deleteflag == false
                              && e.Deleteflag == false
                        select new
                        {
                            events = e,
                            venue = ve
                        }
                    )
                    .Distinct()
                    .ToList();

            var searchResult = new SearchListEventsResponseModel
            {
                Events = events.Select(x=> SearchEventResponseModel.FromTbl(x.events, x.venue)).ToList(),
            };

            if (searchResult.Events.Count == 0)
            {
                return Result<SearchListEventsResponseModel>.NotFoundError("No events found matching the search Amount.");
            }

            return Result<SearchListEventsResponseModel>.Success(searchResult);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<SearchListEventsResponseModel>.SystemError(ex.Message);
        }
    }
}