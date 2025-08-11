using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using EventTicketingSystem.CSharp.Domain.Models.Features.UserEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketingSystem.CSharp.Domain.Features.UserEvent;

public class BL_User_Event
{
    private readonly DA_User_Event _da;

    public BL_User_Event(DA_User_Event da)
    {
        _da = da;
    }

    //public async Task<Result<UserEventListResponseModel>> GetTop3Events()
    //{
    //    var respone = await _da.GetTop3EventList();
    //    return respone;
    //}

    //public async Task<Result<UserEventListResponseModel>> GetEventList(int pageNo)
    //{
    //    var response = await _da.GetEventList(pageNo);
    //    return response;
    //}

    public async Task<Result<UserEventListResponseModel>> GetUserEventList (int pageNo)
    {
        var response = await _da.GetUserEvents(pageNo);
        return response;
    }

    public async Task<Result<EventDetailResponseModel>> GetUserEventDetails (string eventCode)
    {
        var response = await _da.GetEventDetails(eventCode);
        return response;
    }
}
