namespace EventTicketingSystem.CSharp.Domain.Features.EventCategory;

public class BL_EventCategory
{
    private readonly DA_EventCategory _dataAccess;

    public BL_EventCategory(DA_EventCategory dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Result<EventCategoryListResponseModel>> List()
    {
        return await _dataAccess.List();
    }

    public async Task<Result<EventCategoryEditResponseModel>> Edit(string eventCategoryCode)
    {
        return await _dataAccess.Edit(eventCategoryCode);
    }

    public async Task<Result<EventCategoryCreateResponseModel>> Create(EventCategoryCreateRequestModel requestModel)
    {
        return await _dataAccess.Create(requestModel);
    }

    public async Task<Result<EventCategoryUpdateResponseModel>> Update(EventCategoryUpdateRequestModel requestModel)
    {
        return await _dataAccess.Update(requestModel);
    }

    public async Task<Result<EventCategoryDeleteResponseModel>> Delete(string eventCategoryCode)
    {
        return await _dataAccess.Delete(eventCategoryCode);
    }
}
