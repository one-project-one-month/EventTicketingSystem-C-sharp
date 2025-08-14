namespace EventTicketingSystem.CSharp.Domain.Features.Venue;

public class BL_Venue
{
    private readonly DA_Venue _dataAccess;

    public BL_Venue(DA_Venue dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Result<VenueListResponseModel>> List()
    {
        return await _dataAccess.List();
    }

    public async Task<Result<VenueEditResponseModel>> Edit(string venueCode)
    {
        return await _dataAccess.Edit(venueCode);
    }

    public async Task<Result<VenueCreateResponseModel>> Create(VenueCreateRequestModel requestModel)
    {
        return await _dataAccess.Create(requestModel);
    }

    public async Task<Result<VenueUpdateResponseModel>> Update(VenueUpdateRequestModel requestModel)
    {
        return await _dataAccess.Update(requestModel);
    }

    public async Task<Result<VenueDeleteResponseModel>> Delete(string venueCode)
    {
        return await _dataAccess.Delete(venueCode);
    }

    public async Task<Result<VenueListResponseModeForUser>> UserVenueList(int pageNo)
    {
        return await _dataAccess.UserVenueList(pageNo);
    }
}