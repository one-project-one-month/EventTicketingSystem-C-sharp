using Azure;
using DocumentFormat.OpenXml.Office.Word;
using EventTicketingSystem.CSharp.Domain.Models.Features.UserEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventTicketingSystem.CSharp.Domain.Features.UserEvent;

public class DA_User_Event
{
    public readonly ILogger<DA_User_Event> _logger;
    public readonly CommonService _commonService;
    public readonly AppDbContext _appDbContext;

    public DA_User_Event(ILogger<DA_User_Event> logger, CommonService commonService, AppDbContext appDbContext)
    {
        _logger = logger;
        _commonService = commonService;
        _appDbContext = appDbContext;
    }

    public async Task<Result<UserEventListResponseModel>> GetUserEvents(int pageNo)
    {
        var response = new Result<UserEventListResponseModel>();
        try
        {
            var pagination = new PaginationModel { PageNo = pageNo, PageSize = 9};

            var top3Events = await _appDbContext.TblEvents
                .Join(_appDbContext.TblVenues,
                ev => ev.Venuecode,
                ve =>  ve.Venuecode,
                (ev, ve) => new {ev, ve})
                .OrderByDescending(x=>x.ev.Soldoutcount)
                .Take(3)
                .ToListAsync();

            var eventList = _appDbContext.TblEvents
                .Join(_appDbContext.TblVenues,
                ev => ev.Venuecode,
                ve => ve.Venuecode,
                (ev, ve) => new { ev, ve })
                .OrderByDescending(x => x.ev.Eventid);

            var paginationList = await eventList.ToPaginatedList(pagination);

            var model = new UserEventListResponseModel()
            {
                Top3Events = top3Events.Select(x =>
                {
                    var topEvents = UserEventResponseModel.FromTblEvent(x.ev);
                    topEvents.Venueimage = x.ve.Venueimage;
                    topEvents.Address = x.ve.Address;
                    return topEvents;
                }).ToList(),

                EventList = paginationList.ItemList.Select(x =>
                {
                    var events = UserEventResponseModel.FromTblEvent(x.ev);
                    events.Venueimage = x.ve.Venueimage;
                    events.Address = x.ve.Address;
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

    #region EventDetail
    public async Task<Result<EventDetailResponseModel>> GetEventDetails(string eventCode)
    {
        var response = new Result<EventDetailResponseModel>();
        try
        {
            var item = await _appDbContext.TblEvents
                .Join(_appDbContext.TblVenues,
                ev => ev.Venuecode,
                ve => ve.Venuecode,
                (ev, ve) => new { ev, ve })
                .FirstOrDefaultAsync(x => x.ev.Eventcode == eventCode);

            if (item is null) return Result<EventDetailResponseModel>.NotFoundError("Event is not found!");

            var ticketTypes = await _appDbContext.TblTickettypes
                .Where(x => x.Eventcode == eventCode)
                .Join(_appDbContext.TblEvents,
                tt => tt.Eventcode,
                ev => ev.Eventcode,
                (tt, ev) => new { tt, ev })
                .Join(_appDbContext.TblTicketprices,
                tev => tev.tt.Tickettypecode,
                tp => tp.Tickettypecode,
                (tev, tp) => new { tev, tp })
                .ToListAsync();

            var model = new EventDetailResponseModel();
            model = EventDetailResponseModel.FromTbl(item.ev, item.ve);
            
            model.TicketTypes = ticketTypes.Select(x =>  new TicketTypeDetail{
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

    //#region Top3EventList 
    //public async Task<Result<UserEventListResponseModel>> GetTop3EventList()
    //{
    //    var respones = new Result<UserEventListResponseModel>();
    //    try
    //    {
    //        var eventList = await _appDbContext.TblEvents
    //            .Join(_appDbContext.TblVenues,
    //            ev => ev.Venuecode,
    //            ve => ve.Venuecode,
    //            (ev, ve) => new { ev, ve })
    //            .Where(x => !x.ev.Deleteflag)
    //            .OrderByDescending(x => x.ev.Soldoutcount)
    //            .Take(3)
    //            .ToListAsync();

    //        var model = new UserEventListResponseModel()
    //        {
    //            EventList = eventList.Select(x =>
    //            {
    //                var events = UserEventListResponseModel.FromTblEvent(x.ev);
    //                events.Address = x.ve.Address;
    //                events.Venueimage = x.ve.Venueimage;
    //                return events;
    //            }).ToList()
    //        };

    //        respones = Result<UserEventListResponseModel>.Success(model);
    //        return respones;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogExceptionError(ex);
    //        respones = Result<UserEventListResponseModel>.SystemError(ex.Message);
    //        return respones;

    //    }
    //}
    //#endregion

    //#region GetEventList
    //public async Task<Result<UserEventListResponseModel>> GetEventList(int pageNo)
    //{
    //    var response = new Result<UserEventListResponseModel>();

    //    try
    //    {
    //        var pagination = new PaginationModel { PageNo = pageNo, PageSize = 9 };

    //        var eventList = _appDbContext.TblEvents
    //            .Join(_appDbContext.TblVenues,
    //            ev => ev.Venuecode,
    //            ve => ve.Venuecode,
    //            (ev, ve) => new { ev, ve })
    //            .Where(x => !x.ev.Deleteflag)
    //            .OrderByDescending(x => x.ev.Eventid);

    //        var paginationResult = await eventList.ToPaginatedList(pagination);

    //        var model = new UserEventListResponseModel()
    //        {
    //            EventList = paginationResult.ItemList.Select(x =>
    //            {
    //                var events = UserEventListResponseModel.FromTblEvent(x.ev);
    //                events.Address = x.ve.Address;
    //                events.Venueimage = x.ve.Venueimage;
    //                return events;
    //            }).ToList(),
    //            PageNo = paginationResult.Pagination.PageNo,
    //            PageSize = paginationResult.Pagination.PageSize,
    //            TotalRowCount = paginationResult.Pagination.TotalRowCount,

    //        };

    //        response = Result<UserEventListResponseModel>.Success(model); 
    //        return response;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogExceptionError(ex);
    //        response = Result<UserEventListResponseModel>.SystemError(ex.Message);
    //        return response;
    //    }
    //}
    //#endregion

    #region EventDetail

    #endregion
}
