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
        try
        {
            var query = from e in _db.TblEvents
                        join b in _db.TblBusinessowners on e.Businessownercode equals b.Businessownercode
                        where !e.Deleteflag
                        orderby e.Eventid descending
                        select new
                        {
                            EventEntity = e,
                            BusinessOwnerName = b.Fullname
                        };

            var eventList = await query.ToListAsync();

            if (!eventList.Any() || eventList is null)
                return Result<EventListResponseModel>.NotFoundError("No event found.");

            var model = new EventListResponseModel
            {
                EventList = eventList.Select(x =>
                {
                    var eventModel = EventListModel.FromTblEvent(x.EventEntity);
                    eventModel.Businessownername = x.BusinessOwnerName;
                    return eventModel;
                }).ToList(),
            };

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

            var item = await (
                from e in _db.TblEvents.AsNoTracking()
                where !e.Deleteflag && e.Eventcode == eventCode
                join c in _db.TblEventcategories.AsNoTracking()
                    on e.Eventcategorycode equals c.Eventcategorycode into category
                from c in category.DefaultIfEmpty()

                join b in _db.TblBusinessowners.AsNoTracking()
                    on e.Businessownercode equals b.Businessownercode into businessowner
                from b in businessowner.DefaultIfEmpty()

                join v in _db.TblVenues.AsNoTracking()
                    on e.Venuecode equals v.Venuecode into venue
                from v in venue.DefaultIfEmpty()

                join vt in _db.TblVenuetypes.AsNoTracking()
                    on v.Venuetypecode equals vt.Venuetypecode into venuetype
                from vt in venuetype.DefaultIfEmpty()

                orderby e.Eventid descending
                select new
                {
                    e,
                    Categoryname = c != null ? c.Categoryname : string.Empty,
                    Fullname = b != null ? b.Fullname : string.Empty,
                    Venuename = v != null ? v.Venuename : string.Empty,
                    Capacity = v.Capacity,
                    Description = v != null ? v.Description : string.Empty,
                    Facilities = v != null ? v.Facilities : string.Empty,
                    Addons = v != null ? v.Addons : string.Empty,
                    Venueimage = v != null ? v.Venueimage : string.Empty,
                    Address = v != null ? v.Address : string.Empty,
                    Venuetypename = vt != null ? vt.Venuetypename : string.Empty
                }
                ).FirstOrDefaultAsync();

            if (item is null)
            {
                return Result<EventEditResponseModel>.NotFoundError("Event not found.");
            }

            var eventModel = EventEditModel.FromTblEvent(item!.e);
            eventModel.Businessownername = item.Fullname;
            eventModel.Venuename = item.Venuename;
            eventModel.Capacity = item.Capacity;
            eventModel.Description = item.Description;
            eventModel.Facilities = item.Facilities;
            eventModel.Address = item.Address;
            eventModel.Venuetypename = item.Venuetypename;
            eventModel.Eventcategory = item.Categoryname;

            if (!string.IsNullOrWhiteSpace(item.Addons))
            {
                eventModel.Addons = item.Addons
                    .Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(item.Venueimage))
            {
                eventModel.VenueImage = item.Venueimage
                    .Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }

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

            var newEventSeq = new TblSequence()
            {
                Uniquename = requestModel.Uniquename,
                Sequenceno = "000000",
                Sequencedate = DateTime.Now,
                Sequencetype = EnumSequenceType.EventTicket.ToString(),
                Eventcode = newEvent.Eventcode,
                Deleteflag = false
            };
            var newTransactionSeq = new TblSequence()
            {
                Uniquename = requestModel.Uniquename,
                Sequenceno = "000000",
                Sequencedate = DateTime.Now,
                Sequencetype = EnumSequenceType.Transaction.ToString(),
                Eventcode = newEvent.Eventcode,
                Deleteflag = false
            };
            await _db.AddRangeAsync(newEventSeq, newTransactionSeq);
            await _db.SaveAndDetachAsync();

            return Result<EventCreateResponseModel>.Success("Event is successfully Created.");
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

            item.Isactive = requestModel.Isactive;
            item.Eventstatus = requestModel.Eventstatus;
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
            var sequenceList = await _db.TblSequences
                        .Where(
                            x => x.Eventcode == eventCode &&
                            x.Deleteflag == false)
                        .ToListAsync();

            foreach (var sequence in sequenceList)
            {
                sequence.Deleteflag = true;
                _db.Entry(sequence).State = EntityState.Modified;
            }

            await _db.SaveAndDetachAsync();

            var item = await _db.TblEvents
                        .FirstOrDefaultAsync(
                            x => x.Eventcode == eventCode &&
                            x.Deleteflag == false
                        );
            if (item is null)
            {
                return Result<EventDeleteResponseModel>.NotFoundError("Event Not Found.");
            }

            if (Enum.TryParse<EnumEventStatus>(item.Eventstatus, out var status))
            {
                item.Eventstatus = status switch
                {
                    EnumEventStatus.Ongoing => EnumEventStatus.Completed.ToString(),
                    EnumEventStatus.Upcoming => EnumEventStatus.Cancelled.ToString(),
                    _ => item.Eventstatus
                };
            }

            item.Isactive = false;
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

    public List<OptionItem> GetEventStatusOptions()
    {
        return Enum.GetValues<EnumEventStatus>()
            .Select(e => new OptionItem
            {
                Value = e.ToString(),
                Label = e.ToString()
            })
            .ToList();
    }
}