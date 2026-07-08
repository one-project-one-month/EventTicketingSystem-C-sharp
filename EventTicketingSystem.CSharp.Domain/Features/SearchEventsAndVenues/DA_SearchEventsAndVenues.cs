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

            var EventResult = await _db.TblEvents
                            .Where(
                                e => EF.Functions.Like(e.Eventname ?? string.Empty, searchPattern) &&
                                e.Deleteflag == false
                            )
            .Select(e => new SearchEventResponseModel
            {
                Eventid = e.Eventid,
                Eventcode = e.Eventcode,
                Eventname = e.Eventname,
                Categorycode = e.Eventcategorycode,
                Startdate = e.Startdate,
                Enddate = e.Enddate,
                Isactive = e.Isactive,
                Eventstatus = e.Eventstatus,
                Businessownercode = e.Businessownercode,
                Totalticketquantity = e.Totalticketquantity,
                Soldoutcount = e.Soldoutcount,
                Createdby = e.Createdby,
                Createdat = e.Createdat,
                Modifiedby = e.Modifiedby,
                Modifiedat = e.Modifiedat
            })
            .ToListAsync();

            var VenueResult = await _db.TblVenues
                            .Where(
                                v => EF.Functions.Like(v.Venuename ?? string.Empty, searchPattern) &&
                                v.Deleteflag == false
                            )
                .Select(v => new SearchVenuesResponseModel
                {
                    Venueid = v.Venueid,
                    Venuecode = v.Venuecode,
                    Venuename = v.Venuename,
                    //Venuedetailcode = v.Venuedetailcode,
                    Venuetypecode = v.Venuetypecode,
                    Venuedescription = v.Description,
                    Venueaddress = v.Address,
                    Venuecapacity = v.Capacity,
                    Venuefacilities = v.Facilities,
                    Venueaddons = v.Addons,
                    Createdby = v.Createdby,
                    Createdat = v.Createdat,
                    Modifiedby = v.Modifiedby,
                    Modifiedat = v.Modifiedat
                })
                .ToListAsync();

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

            var EventResult = await _db.TblEvents
                            .Where(
                                e => e.Startdate >= Startdate &&
                                e.Startdate <= nextDay &&
                                e.Deleteflag == false
                            )
                            .Select(e => new SearchEventResponseModel
                            {
                                Eventid = e.Eventid,
                                Eventcode = e.Eventcode,
                                Eventname = e.Eventname,
                                Categorycode = e.Eventcategorycode,
                                Startdate = e.Startdate,
                                Enddate = e.Enddate,
                                Isactive = e.Isactive,
                                Eventstatus = e.Eventstatus,
                                Businessownercode = e.Businessownercode,
                                Totalticketquantity = e.Totalticketquantity,
                                Soldoutcount = e.Soldoutcount,
                                Createdby = e.Createdby,
                                Createdat = e.Createdat,
                                Modifiedby = e.Modifiedby,
                                Modifiedat = e.Modifiedat
                            })
                            .ToListAsync();

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
                Events = await _db.TblEvents
                        .Where(
                            e => eventCodes.Contains(e.Eventcode) &&
                            e.Deleteflag == false
                        )
                        .Select(e => new SearchEventByAmountResponseModel
                        {
                            Eventid = e.Eventid,
                            Eventcode = e.Eventcode,
                            Eventname = e.Eventname,
                            Categorycode = e.Eventcategorycode,
                            Description = e.Description,
                            Address = e.Address,
                            Startdate = e.Startdate,
                            Enddate = e.Enddate,
                            Isactive = e.Isactive,
                            Eventstatus = e.Eventstatus,
                            Businessownercode = e.Businessownercode,
                            Totalticketquantity = e.Totalticketquantity,
                            Soldoutcount = e.Soldoutcount,
                            Createdby = e.Createdby,
                            Createdat = e.Createdat,
                            Modifiedby = e.Modifiedby,
                            Modifiedat = e.Modifiedat
                        })
                        .ToListAsync()
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
}
