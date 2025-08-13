namespace EventTicketingSystem.CSharp.Domain.Features.BusinessOwner;

public class BL_BusinessOwner
{
    private readonly DA_BusinessOwner _daService;

    public BL_BusinessOwner(DA_BusinessOwner dataAccess)
    {
        _daService = dataAccess;
    }

    public async Task<Result<BusinessOwnerListResponseModel>> List()
    {
        return await _daService.List();
    }

    public async Task<Result<BusinessOwnerEditResponseModel>> Edit(string ownerCode)
    {
        return await _daService.Edit(ownerCode);
    }

    public async Task<Result<BusinessOwnerCreateResponseMOdel>> Create(BusinessOwnerCreateRequestModel requestModel)
    {
        return await _daService.Create(requestModel);
    }

    public async Task<Result<BusinessOwnerUpdateResponseMOdel>> Update(BusinessOwnerUpdateRequestModel requestModel)
    {
        return await _daService.Update(requestModel);
    }

    public async Task<Result<BusinessOwnerDeleteResponseMOdel>> Delete(string ownerCode)
    {
        return await _daService.Delete(ownerCode);
    }
}