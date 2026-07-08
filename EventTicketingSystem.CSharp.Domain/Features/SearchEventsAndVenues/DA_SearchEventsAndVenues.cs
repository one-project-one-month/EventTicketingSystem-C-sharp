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
            var searchPattern = $"%{searchTerm?.Trim()}%";

            var eventRows = await (
                from evt in _db.TblEvents
                join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
                where EF.Functions.Like(evt.Eventname ?? string.Empty, searchPattern)
                      && evt.Deleteflag == false
                      && venue.Deleteflag == false
                select new
                {
                    evt,
                    venue.Address,
                    venue.Venueimage
                }).ToListAsync();

            var EventResult = eventRows.Select(x => new SearchEventResponseModel
            {
                Eventid = x.evt.Eventid,
                Eventcode = x.evt.Eventcode,
                Eventname = x.evt.Eventname,
                Categorycode = x.evt.Eventcategorycode,
                Address = x.Address,
                Venueimage = SplitCsv(x.Venueimage),
                Startdate = x.evt.Startdate,
                Enddate = x.evt.Enddate,
                Isactive = x.evt.Isactive,
                Eventstatus = x.evt.Eventstatus,
                Businessownercode = x.evt.Businessownercode,
                Totalticketquantity = x.evt.Totalticketquantity,
                Soldoutcount = x.evt.Soldoutcount,
                Createdby = x.evt.Createdby,
                Createdat = x.evt.Createdat,
                Modifiedby = x.evt.Modifiedby,
                Modifiedat = x.evt.Modifiedat
            }).ToList();

            var venueRows = await (
                from venue in _db.TblVenues
                join venueType in _db.TblVenuetypes on venue.Venuetypecode equals venueType.Venuetypecode
                where EF.Functions.Like(venue.Venuename ?? string.Empty, searchPattern)
                      && venue.Deleteflag == false
                      && venueType.Deleteflag == false
                select new { venue, venueType.Venuetypename })
                .ToListAsync();

            var VenueResult = venueRows
                .Select(v => new SearchVenuesResponseModel
                {
                    Venueid = v.venue.Venueid,
                    Venuecode = v.venue.Venuecode,
                    Venuename = v.venue.Venuename,
                    //Venuedetailcode = v.Venuedetailcode,
                    Venuetypecode = v.venue.Venuetypecode,
                    Venuetypename = v.Venuetypename,
                    Venuedescription = v.venue.Description,
                    Venueaddress = v.venue.Address,
                    Address = v.venue.Address,
                    Venuecapacity = v.venue.Capacity,
                    Capacity = v.venue.Capacity,
                    Venueimage = SplitCsv(v.venue.Venueimage),
                    Venuefacilities = v.venue.Facilities,
                    Venueaddons = v.venue.Addons,
                    Createdby = v.venue.Createdby,
                    Createdat = v.venue.Createdat,
                    Modifiedby = v.venue.Modifiedby,
                    Modifiedat = v.venue.Modifiedat
                })
                .ToList();

            var SearchResult = new SearchListEventsAndVenuesResponseModel
            {
                Events = EventResult,
                Venues = VenueResult
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
            // Add day 1 to include 23:59:59 on the original EndDate
            var nextDay = Enddate.AddDays(1);

            var eventRows = await (
                from evt in _db.TblEvents
                join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
                where evt.Startdate >= Startdate
                      && evt.Startdate <= nextDay
                      && evt.Deleteflag == false
                      && venue.Deleteflag == false
                select new { evt, venue.Address, venue.Venueimage })
                .ToListAsync();

            var EventResult = eventRows
                            .Select(e => new SearchEventResponseModel
                            {
                                Eventid = e.evt.Eventid,
                                Eventcode = e.evt.Eventcode,
                                Eventname = e.evt.Eventname,
                                Categorycode = e.evt.Eventcategorycode,
                                Address = e.Address,
                                Venueimage = SplitCsv(e.Venueimage),
                                Startdate = e.evt.Startdate,
                                Enddate = e.evt.Enddate,
                                Isactive = e.evt.Isactive,
                                Eventstatus = e.evt.Eventstatus,
                                Businessownercode = e.evt.Businessownercode,
                                Totalticketquantity = e.evt.Totalticketquantity,
                                Soldoutcount = e.evt.Soldoutcount,
                                Createdby = e.evt.Createdby,
                                Createdat = e.evt.Createdat,
                                Modifiedby = e.evt.Modifiedby,
                                Modifiedat = e.evt.Modifiedat
                            })
                            .ToList();

            var SearchResult = new SearchListEventsResponseModel
            {
                Events = EventResult
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

    public async Task<Result<SearchListEventsByAmountResponseModel>> SearchEventsByAmountAsync(decimal FromAmount, decimal ToAmount)
    {
        try
        {
            var eventCodes = await (from ticketPrice in _db.TblTicketprices
                                    join ticketType in _db.TblTickettypes
                                        on ticketPrice.Tickettypecode equals ticketType.Tickettypecode
                                    where ticketPrice.Ticketprice >= FromAmount
                                          && ticketPrice.Ticketprice <= ToAmount
                                          && ticketPrice.Deleteflag == false
                                          && ticketType.Deleteflag == false
                                    select ticketType.Eventcode)
                .Distinct()
                .ToListAsync();

            var SearchResult = new SearchListEventsByAmountResponseModel
            {
                Events = (await (
                            from evt in _db.TblEvents
                            join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
                            where eventCodes.Contains(evt.Eventcode)
                                  && evt.Deleteflag == false
                                  && venue.Deleteflag == false
                            select new { evt, venue.Venueimage })
                        .ToListAsync())
                        .Select(e => new SearchEventByAmountResponseModel
                        {
                            Eventid = e.evt.Eventid,
                            Eventcode = e.evt.Eventcode,
                            Eventname = e.evt.Eventname,
                            Categorycode = e.evt.Eventcategorycode,
                            Description = e.evt.Description,
                            Address = e.evt.Address,
                            Venueimage = SplitCsv(e.Venueimage),
                            Startdate = e.evt.Startdate,
                            Enddate = e.evt.Enddate,
                            Isactive = e.evt.Isactive,
                            Eventstatus = e.evt.Eventstatus,
                            Businessownercode = e.evt.Businessownercode,
                            Totalticketquantity = e.evt.Totalticketquantity,
                            Soldoutcount = e.evt.Soldoutcount,
                            Createdby = e.evt.Createdby,
                            Createdat = e.evt.Createdat,
                            Modifiedby = e.evt.Modifiedby,
                            Modifiedat = e.evt.Modifiedat
                        })
                        .ToList()
            };

            if (SearchResult.Events.Count == 0)
            {
                return Result<SearchListEventsByAmountResponseModel>.NotFoundError("No events found matching the search Amount.");
            }

            return Result<SearchListEventsByAmountResponseModel>.Success(SearchResult);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<SearchListEventsByAmountResponseModel>.SystemError(ex.Message);
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
