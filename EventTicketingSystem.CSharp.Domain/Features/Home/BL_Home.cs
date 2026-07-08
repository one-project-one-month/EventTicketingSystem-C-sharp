namespace EventTicketingSystem.CSharp.Domain.Features.Home;

public class BL_Home
{
    private readonly DA_Home _dataAccess;

    public BL_Home(DA_Home dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Result<HomeResponseModel>> GetHome()
    {
        return await _dataAccess.GetHome();
    }
}
