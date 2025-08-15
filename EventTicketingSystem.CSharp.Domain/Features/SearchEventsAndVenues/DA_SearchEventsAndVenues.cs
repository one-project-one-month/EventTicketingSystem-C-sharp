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
            .Select(e => new SearchEventResponseModel
            {
                Eventid = e.Eventid,
                Eventcode = e.Eventcode,
                Eventname = e.Eventname,
                Categorycode = e.Eventcategorycode,
                Startdate = e.Startdate,
                Enddate = e.Enddate,
                Isactive = e.Isactive,
                Eventstatus = e.Eventcode,
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
                                v => EF.Functions.ILike(v.Venuename!, "%" + searchTerm + "%") &&
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
            //var nextDay = Enddate.AddDays(1);
            
            var startUtc = DateTime.SpecifyKind(Startdate.Date, DateTimeKind.Utc);
            var endUtc   = DateTime.SpecifyKind(Enddate.Date.AddDays(1), DateTimeKind.Utc);

            var EventResult = await _db.TblEvents
                .Where(e =>
                    e.Startdate >= startUtc &&
                    e.Enddate < endUtc &&
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
            var searchResult = new SearchListEventsByAmountResponseModel
            {
                Events = (
                        from tp in _db.TblTicketprices
                        join tt in _db.TblTickettypes on tp.Tickettypecode equals tt.Tickettypecode
                        join e in _db.TblEvents on tt.Eventcode equals e.Eventcode
                        where tp.Ticketprice >= FromAmount
                              && tp.Ticketprice <= ToAmount
                              && tp.Deleteflag == false
                              && tt.Deleteflag == false 
                              && e.Deleteflag == false
                        select new SearchEventByAmountResponseModel
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
                        }
                    )
                    .Distinct() 
                    .ToList()
            };

            if (searchResult.Events.Count == 0)
            {
                return Result<SearchListEventsByAmountResponseModel>.NotFoundError("No events found matching the search Amount.");
            }

            return Result<SearchListEventsByAmountResponseModel>.Success(searchResult);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<SearchListEventsByAmountResponseModel>.SystemError(ex.Message);
        }
    }
}