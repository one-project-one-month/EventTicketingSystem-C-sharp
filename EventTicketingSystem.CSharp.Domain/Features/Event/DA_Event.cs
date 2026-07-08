namespace EventTicketingSystem.CSharp.Domain.Features.Event;

public class DA_Event : AuthorizationService
{
    private readonly ILogger<DA_Event> _logger;
    private readonly AppDbContext _db;
    private readonly CommonService _commonService;

    public DA_Event(IHttpContextAccessor httpContextAccessor,
                    ILogger<DA_Event> logger,
                    AppDbContext db,
                    CommonService commonService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _commonService = commonService;
    }

    #region Event List

    public async Task<Result<EventListResponseModel>> List()
    {
        var responseModel = new Result<EventListResponseModel>();
        var model = new EventListResponseModel();

        try
        {
            var data = await (from e in _db.TblEvents
                join b in _db.TblBusinessowners on e.Businessownercode equals b.Businessownercode
                where e.Deleteflag == false
                orderby e.Eventid descending
                select new
                {
                    e,
                    b.Fullname
                }).ToListAsync();
            
            if (data is null || !data.Any())
            {
                return Result<EventListResponseModel>.NotFoundError("No event found.");
            }
            
            model.EventList = data.Select(x =>
            {
                var eventModel = EventListModel.FromTblEvent(x.e); 
                eventModel.Businessownername = x.Fullname;
                return eventModel;
            }).ToList();
            
            return Result<EventListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventListResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Event Edit

    public async Task<Result<EventEditResponseModel>> Edit(string eventCode)
    {
        var model = new EventEditResponseModel();

        if (eventCode.IsNullOrEmpty())
        {
            return Result<EventEditResponseModel>.UserInputError("Event code required.");
        }

        try
        {
            
            var item = await (from e in _db.TblEvents
                join b in _db.TblBusinessowners on e.Businessownercode equals b.Businessownercode
                join v in _db.TblVenues on e.Venuecode equals v.Venuecode
                join vt in _db.TblVenuetypes on v.Venuetypecode equals vt.Venuetypecode
                where e.Deleteflag == false && e.Eventcode == eventCode
                orderby e.Eventid descending
                select new
                {
                    e,
                    b.Fullname,
                    v.Venuename, v.Capacity, v.Description, v.Facilities, v.Addons, v.Venueimage,v.Address,
                    vt.Venuetypename
                }).ToListAsync();
            
            if (!item.Any())
            {
                return Result<EventEditResponseModel>.NotFoundError("No event found.");
            }
            
            var firstItem = item.First();

            var eventModel = EventEditModel.FromTblEvent(firstItem.e);
            eventModel.Businessownername = firstItem.Fullname;
            eventModel.Venuename = firstItem.Venuename;
            eventModel.Capacity = firstItem.Capacity;
            eventModel.Description = firstItem.Description;
            eventModel.Facilities = firstItem.Facilities;
            
            if (!string.IsNullOrWhiteSpace(firstItem.Addons))
            {
                eventModel.Addons = firstItem.Addons
                    .Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(firstItem.Venueimage))
            {
                eventModel.VenueImage = firstItem.Venueimage
                    .Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }
            
            eventModel.Address = firstItem.Address;
            eventModel.Venuetypename = firstItem.Venuetypename;

            model.Event = eventModel;            
            
            return Result<EventEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventEditResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Event Create

    public async Task<Result<EventCreateResponseModel>> Create(EventCreateRequestModel requestModel)
    {
        if (requestModel.Eventname.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Event name is required!");
        }
        if (requestModel.Uniquename.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Unique name is required!");
        }
        if (requestModel.Eventcategorycode.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Event category is required");
        }
        if (requestModel.Businessownercode.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Business owner is required");
        }
        if (requestModel.Venuecode.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Venue is required!");
        }
        if (requestModel.Totalticketquantity.IsNullOrEmpty() || requestModel.Totalticketquantity <= 0)
        {
            return Result<EventCreateResponseModel>.ValidationError("Invalid Ticket Quantity!");
        }
        if (requestModel.Startdate.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Start date is required!");
        }
        if (requestModel.Enddate.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("End date is required!");
        }
        if (requestModel.Isactive.IsNullOrEmpty())
        {
            return Result<EventCreateResponseModel>.ValidationError("Is active is required");
        }

        try
        {
            var newEvent = new TblEvent()
            {
                Eventid = GenerateUlid(),
                Eventcode = await _commonService.GenerateSequenceCode(EnumTableUniqueName.Tbl_Event),
                Description = await _db.TblVenues.Where(x => x.Venuecode == requestModel.Venuecode).Select(x => x.Description ?? string.Empty).FirstOrDefaultAsync() ?? string.Empty,
                Address = await _db.TblVenues.Where(x => x.Venuecode == requestModel.Venuecode).Select(x => x.Address).FirstOrDefaultAsync() ?? string.Empty,
                Eventname = requestModel.Eventname,
                Uniquename = requestModel.Uniquename,
                Eventcategorycode = requestModel.Eventcategorycode,
                Businessownercode = requestModel.Businessownercode,
                Venuecode = requestModel.Venuecode,
                Totalticketquantity = requestModel.Totalticketquantity,
                Startdate = requestModel.Startdate,
                Enddate = requestModel.Enddate,
                Isactive = true,
                Eventstatus = EnumEventStatus.Upcoming.ToString(),
                Soldoutcount = 0,
                Createdby = CurrentUserId,
                Createdat = DateTime.Now,
                Deleteflag = false
            };
            await _db.AddAsync(newEvent);
            await _db.SaveAndDetachAsync();

            return Result<EventCreateResponseModel>.Success("Event is successfully Created");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventCreateResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Event Update

    public async Task<Result<EventUpdateResponseModel>> Update(EventUpdateRequestModel requestModel)
    {
        try
        {
            var item = await _db.TblEvents
                        .FirstOrDefaultAsync(
                            x => x.Eventcode == requestModel.EventCode &&
                            x.Deleteflag == false
                        );
            if (item is null)
            {
                return Result<EventUpdateResponseModel>.NotFoundError("Event Not Found.");
            }

            if (!requestModel.EventName.IsNullOrEmpty())
            {
                item.Eventname = requestModel.EventName;
            }
            if (requestModel.Startdate != default)
            {
                item.Startdate = requestModel.Startdate;
            }
            if (requestModel.Enddate != default)
            {
                item.Enddate = requestModel.Enddate;
            }
            item.Isactive = requestModel.Isactive;
            if (!requestModel.Eventstatus.IsNullOrEmpty())
            {
                item.Eventstatus = requestModel.Eventstatus;
            }
            if (requestModel.Totalticketquantity > 0)
            {
                item.Totalticketquantity = requestModel.Totalticketquantity;
            }
            item.Modifiedby = CurrentUserId;
            item.Modifiedat = DateTime.Now;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<EventUpdateResponseModel>.Success("Event Updated Successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventUpdateResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region Event Delete

    public async Task<Result<EventDeleteResponseModel>> Delete(string eventCode)
    {
        if (eventCode.IsNullOrEmpty())
        {
            return Result<EventDeleteResponseModel>.ValidationError("Event Code Required.");
        }

        try
        {
            var item = await _db.TblEvents
                        .FirstOrDefaultAsync(
                            x => x.Eventcode == eventCode &&
                            x.Deleteflag == false
                        );
            if (item is null)
            {
                return Result<EventDeleteResponseModel>.NotFoundError("Event Not Found.");
            }

            item.Deleteflag = true;
            item.Modifiedby = CurrentUserId;
            item.Modifiedat = DateTime.Now;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            return Result<EventDeleteResponseModel>.Success("Event Deleted Successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EventDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    #endregion

    #region User Events

    public async Task<Result<UserEventListResponseModel>> UserEvents(int pageNo)
    {
        const int pageSize = 9;
        pageNo = pageNo <= 0 ? 1 : pageNo;

        try
        {
            var query =
                from evt in _db.TblEvents
                join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
                where evt.Deleteflag == false && venue.Deleteflag == false && evt.Isactive
                orderby evt.Startdate
                select new
                {
                    evt.Eventcode,
                    evt.Eventname,
                    venue.Address,
                    venue.Venueimage
                };

            var total = await query.CountAsync();
            var rows = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            var topRows = await query.Take(3).ToListAsync();

            var model = new UserEventListResponseModel
            {
                EventList = rows.Select(x => new UserEventListModel
                {
                    Eventcode = x.Eventcode,
                    Eventname = x.Eventname,
                    Address = x.Address,
                    Venueimage = SplitCsv(x.Venueimage)
                }).ToList(),
                Top3Events = topRows.Select(x => new UserEventListModel
                {
                    Eventcode = x.Eventcode,
                    Eventname = x.Eventname,
                    Address = x.Address,
                    Venueimage = SplitCsv(x.Venueimage)
                }).ToList(),
                TotalRowCount = total,
                PageNo = pageNo,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

            return Result<UserEventListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<UserEventListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<UserEventDetailResponseModel>> UserEventDetails(string eventCode)
    {
        try
        {
            var detail = await (
                from evt in _db.TblEvents
                join venue in _db.TblVenues on evt.Venuecode equals venue.Venuecode
                where evt.Deleteflag == false && venue.Deleteflag == false && evt.Eventcode == eventCode
                select new
                {
                    evt.Eventcode,
                    evt.Eventname,
                    evt.Startdate,
                    evt.Enddate,
                    venue.Venuename,
                    venue.Description,
                    venue.Address,
                    venue.Capacity,
                    venue.Facilities,
                    venue.Addons,
                    venue.Venueimage
                }).FirstOrDefaultAsync();

            if (detail is null)
            {
                return Result<UserEventDetailResponseModel>.NotFoundError("Event not found.");
            }

            var ticketTypes = await (
                from type in _db.TblTickettypes
                join price in _db.TblTicketprices on type.Tickettypecode equals price.Tickettypecode
                where type.Deleteflag == false && price.Deleteflag == false && type.Eventcode == eventCode
                select new UserEventTicketTypeModel
                {
                    Tickettypecode = type.Tickettypecode,
                    Tickettypename = type.Tickettypename,
                    Ticketprice = price.Ticketprice
                }).ToListAsync();

            var model = new UserEventDetailResponseModel
            {
                Eventcode = detail.Eventcode,
                Eventname = detail.Eventname,
                Startdate = detail.Startdate,
                Enddate = detail.Enddate,
                Venuename = detail.Venuename,
                Description = detail.Description ?? string.Empty,
                Address = detail.Address,
                Capacity = detail.Capacity,
                Facilities = detail.Facilities ?? string.Empty,
                Addons = detail.Addons ?? string.Empty,
                Venueimage = SplitCsv(detail.Venueimage),
                TicketTypes = ticketTypes
            };

            return Result<UserEventDetailResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<UserEventDetailResponseModel>.SystemError(ex.Message);
        }
    }

    public Result<EventStatusOptionsResponseModel> EventStatusOptions()
    {
        var options = Enum.GetNames<EnumEventStatus>()
            .Select(x => new EventStatusOptionModel { Value = x, Label = x })
            .ToList();

        return Result<EventStatusOptionsResponseModel>.Success(new EventStatusOptionsResponseModel
        {
            EventStatusOptions = options
        });
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

    #endregion
}
