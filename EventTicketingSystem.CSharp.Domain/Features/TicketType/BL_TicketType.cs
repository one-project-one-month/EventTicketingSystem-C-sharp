namespace EventTicketingSystem.CSharp.Domain.Features.TicketType;

public class BL_TicketType
{
    private readonly DA_TicketType _da_ticketType;

    public BL_TicketType(DA_TicketType da_ticketType)
    {
        _da_ticketType = da_ticketType;
    }

    public async Task<Result<TicketTypeListResponseModel>> List()
    {
        var lst = await _da_ticketType.List();
        return lst;
    }

    public async Task<Result<TicketTypeEditResponseModel>> Edit(string code)
    {
        var ticketType = await _da_ticketType.Edit(code);
        return ticketType;
    }

    public async Task<Result<TicketTypeCreateResponseModel>> Create(TicketTypeCreateRequestModel requestModel)
    {
        var result = await _da_ticketType.Create(requestModel);
        return result;
    }

    public async Task<Result<TicketTypeUpdateResponseModel>> Update(TicketTypeUpdateRequestModel requestModel)
    {
        var result = await _da_ticketType.Update(requestModel);
        return result;
    }

    public async Task<Result<TicketTypeDeleteResponseModel>> Delete(string code)
    {
        var result = await _da_ticketType.Delete(code);
        return result;
    }
}