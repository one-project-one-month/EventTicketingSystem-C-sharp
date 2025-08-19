namespace EventTicketingSystem.CSharp.Domain.Features.UserEvent;

public class DA_User_Event
{
    public readonly ILogger<DA_User_Event> _logger;
    public readonly CommonService _commonService;
    public readonly AppDbContext _db;

    public DA_User_Event(ILogger<DA_User_Event> logger,
                         CommonService commonService,
                         AppDbContext db)
    {
        _logger = logger;
        _commonService = commonService;
        _db = db;
    }

    #region UserEvents

    public async Task<Result<UserEventListResponseModel>> GetUserEvents(int pageNo)
    {
        var response = new Result<UserEventListResponseModel>();
        try
        {
            var pagination = new PaginationModel { PageNo = pageNo, PageSize = 9 };

            var top3Events = await _db.TblEvents
                .Join(_db.TblVenues,
                ev => ev.Venuecode,
                ve => ve.Venuecode,
                (ev, ve) => new { ev, ve })
                .Where(
                    x => (x.ev.Eventstatus == EnumEventStatus.Upcoming.ToString() || 
                    x.ev.Eventstatus == EnumEventStatus.Ongoing.ToString()) && 
                    x.ev.Isactive == true)
                .OrderByDescending(x => x.ev.Soldoutcount)
                .Take(3)
                .ToListAsync();

            var eventList = _db.TblEvents
                .Join(_db.TblVenues,
                ev => ev.Venuecode,
                ve => ve.Venuecode,
                (ev, ve) => new { ev, ve })
                .Where(
                    x => (x.ev.Eventstatus == EnumEventStatus.Upcoming.ToString() ||
                    x.ev.Eventstatus == EnumEventStatus.Ongoing.ToString()) &&
                    x.ev.Isactive == true)
                .OrderByDescending(x => x.ev.Eventid);

            var paginationList = await eventList.ToPaginatedList(pagination);

            var model = new UserEventListResponseModel()
            {
                Top3Events = top3Events.Select(x =>
                {
                    var topEvents = UserEventResponseModel.FromTblEvent(x.ev, x.ve);
                    return topEvents;
                }).ToList(),

                EventList = paginationList.ItemList.Select(x =>
                {
                    var events = UserEventResponseModel.FromTblEvent(x.ev, x.ve);
                    return events;
                }).ToList(),

                PageNo = paginationList.Pagination.PageNo,
                PageSize = paginationList.Pagination.PageSize,
                TotalRowCount = paginationList.Pagination.TotalRowCount
            };

            response = Result<UserEventListResponseModel>.Success(model);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            response = Result<UserEventListResponseModel>.SystemError(ex.Message);
            return response;
        }
    }

    #endregion

    #region EventDetail

    public async Task<Result<EventDetailResponseModel>> GetEventDetails(string eventCode)
    {
        var response = new Result<EventDetailResponseModel>();
        var model = new EventDetailResponseModel();
        try
        {
            var item = await _db.TblEvents
                .Join(_db.TblVenues,
                ev => ev.Venuecode,
                ve => ve.Venuecode,
                (ev, ve) => new { ev, ve })
                .FirstOrDefaultAsync(x => x.ev.Eventcode == eventCode);

            if (item is null) return Result<EventDetailResponseModel>.NotFoundError("Event is not found!");

            var ticketTypes = await _db.TblTickettypes
                .Where(x => x.Eventcode == eventCode)
                .Join(_db.TblEvents,
                tt => tt.Eventcode,
                ev => ev.Eventcode,
                (tt, ev) => new { tt, ev })
                .Join(_db.TblTicketprices,
                tev => tev.tt.Tickettypecode,
                tp => tp.Tickettypecode,
                (tev, tp) => new { tev, tp })
                .ToListAsync();

            model = EventDetailResponseModel.FromTbl(item.ev, item.ve);

            model.TicketTypes = ticketTypes.Select(x => new TicketTypeDetail
            {
                Tickettypecode = x.tev.tt.Tickettypecode,
                Tickettypename = x.tev.tt.Tickettypename,
                Ticketprice = x.tp.Ticketprice,
            }).ToList();

            response = Result<EventDetailResponseModel>.Success(model);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            response = Result<EventDetailResponseModel>.SystemError(ex.Message);
            return response;
        }
    }

    #endregion
}