namespace EventTicketingSystem.CSharp.Domain.Features.VerificationCode;

public class BL_VerificationCode
{
    private readonly DA_VerificationCode _daService;

    public BL_VerificationCode(DA_VerificationCode dataAccess)
    {
        _daService = dataAccess;
    }

    public async Task<Result<VCResponseModel>> List(int pageNo)
    {
        return await _daService.List(pageNo);
    }

    public async Task<Result<VCResponseModel>> GetById(string vcId)
    {
        return await _daService.GetVerificationCodeById(vcId);
    }

    public async Task<Result<VCResponseModel>> GetByEmail(string email)
    {
        return await _daService.GetByEmail(email);
    }

    public async Task<Result<VCResponseModel>> Create(VCCreateRequestModel request)
    {
        return await _daService.Create(request);
    }

    public async Task<Result<bool>> VerifyCode(VCRequestModel request)
    {
        return await _daService.VerifyCode(request.Email, request.VerificationCode);
    }
}