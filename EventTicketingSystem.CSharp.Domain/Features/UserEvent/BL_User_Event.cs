namespace EventTicketingSystem.CSharp.Domain.Features.UserEvent;

public class BL_User_Event
{
    private readonly DA_User_Event _da;

    public BL_User_Event(DA_User_Event da)
    {
        _da = da;
    }

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
