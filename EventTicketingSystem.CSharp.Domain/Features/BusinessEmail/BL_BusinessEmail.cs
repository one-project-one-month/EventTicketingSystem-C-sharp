namespace EventTicketingSystem.CSharp.Domain.Features.BusinessEmail;

public class BL_BusinessEmail
{
    private readonly DA_BusinessEmail _da_BusinessEmail;

    public BL_BusinessEmail(DA_BusinessEmail da_BusinessEmail)
    {
        _da_BusinessEmail = da_BusinessEmail;
    }

    public async Task<Result<BusinessEmailCreateResponseModel>> Create(BusinessEmailCreateRequestModel requestModel)
    {
        return await _da_BusinessEmail.Create(requestModel);
    }

    public async Task<Result<BusinessEmailEditResponseModel>> Edit(string businessEmailCode)
    {
        var data = await _da_BusinessEmail.Edit(businessEmailCode);
        return data;
    }

    public async Task<Result<BusinessEmailListResponseModel>> List()
    {
        var data = await _da_BusinessEmail.List();
        return data;
    }
}