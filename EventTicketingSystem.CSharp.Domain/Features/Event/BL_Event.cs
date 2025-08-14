namespace EventTicketingSystem.CSharp.Domain.Features.Event;

public class BL_Event
{
    private readonly DA_Event _daService;

    public BL_Event(DA_Event service)
    {
        _daService = service;
    }

    public async Task<Result<EventListResponseModel>> List()
    {
        return await _daService.List();
    }

    public async Task<Result<EventEditResponseModel>> Edit(string eventCode)
    {
        return await _daService.Edit(eventCode);
    }
    
    public async Task<Result<EventCreateResponseModel>> Create(EventCreateRequestModel requestModel)
    {
        return await _daService.Create(requestModel);
    }

    public async Task<Result<EventUpdateResponseModel>> Update(EventUpdateRequestModel requestModel)
    {
        return await _daService.Update(requestModel);
    }

    public async Task<Result<EventDeleteResponseModel>> Delete(string eventCode)
    {
        return await _daService.Delete(eventCode);
    }
    
    public Result<EventStatusOptionsModel> GetEventStatusOptions()
    {
        var options = _daService.GetEventStatusOptions(); 

        var model = new EventStatusOptionsModel
        {
            EventStatusOptions = options
        };

        return Result<EventStatusOptionsModel>.Success(model, "Retrieved Event Status Options Successfully.");
    }
}