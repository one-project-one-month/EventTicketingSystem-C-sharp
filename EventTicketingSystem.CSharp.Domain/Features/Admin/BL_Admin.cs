namespace EventTicketingSystem.CSharp.Domain.Features.Admin;

public class BL_Admin
{
    private readonly DA_Admin _dataAccess;

    public BL_Admin(DA_Admin dataAccess) => _dataAccess = dataAccess;

    public async Task<Result<AdminListResponseModel>> List()
    {
        var data = await _dataAccess.List();
        return data;
    }

    public async Task<Result<AdminEditResponseModel>> Edit(string adminCode)
    {
        var data = await _dataAccess.Edit(adminCode);
        return data;
    }

    public async Task<Result<AdminCreateResponseModel>> Create(AdminCreateRequestModel requestModel)
    {
        var data = await _dataAccess.Create(requestModel);
        return data;
    }

    public async Task<Result<AdminUpdateResponseModel>> Update(AdminUpdateRequestModel requestModel)
    {
        var data = await _dataAccess.Update(requestModel);
        return data;
    }

    public async Task<Result<AdminDeleteResponseModel>> Delete(AdminDeleteRequestModel requestModel)
    {
        var data = await _dataAccess.Delete(requestModel);
        return data;
    }

    public async Task<Result<AdminEditProfileResponseModel>> EditProfileImage(AdminEditProfileRequestModel requestModel)
    {
        var data = await _dataAccess.EditProfile(requestModel);
        return data;
    }

    public async Task<Result<AdminChangePasswordResponseModel>> ChangePassword(AdminChangePasswordRequestModel requestModel)
    {
        var data = await _dataAccess.ChangePassword(requestModel);
        return data;
    }
}