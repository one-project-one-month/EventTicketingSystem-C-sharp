using EventTicketingSystem.CSharp.Domain.Models.Features.Home;

namespace EventTicketingSystem.CSharp.Domain.Features.Home;

public class BL_Home
{
    private readonly DA_Home _dataAccess;

    public BL_Home(DA_Home dataAccess) => _dataAccess = dataAccess;

    public async Task<Result<HomeResponseModels>> Home()
    { 
        var data = await _dataAccess.Home();
        return data;
    }
}
