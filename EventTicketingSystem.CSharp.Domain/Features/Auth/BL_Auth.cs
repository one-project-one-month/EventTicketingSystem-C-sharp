using Microsoft.Extensions.Options;

namespace EventTicketingSystem.CSharp.Domain.Features.Auth;

public class BL_Auth
{
    private readonly JwtSettings _jwtSettings;
    private readonly JwtService _jwtService;
    private readonly DA_Auth _daAuth;
    private readonly ILogger<BL_Auth> _logger;
    private readonly UserContextService _userContextService;

    public BL_Auth(IOptions<JwtSettings> jwtSettingsOptions, JwtService jwtService,
                        DA_Auth daAuth, UserContextService userContextService, ILogger<BL_Auth> logger)
    {
        _jwtSettings = jwtSettingsOptions.Value;
        _jwtService = jwtService;
        _daAuth = daAuth;
        _userContextService = userContextService;
        _logger = logger;
    }

    public async Task<Result<LoginResponseModel>> Login(LoginRequestModel requestModel)
    {
        var user = await _daAuth.GetUserByUsername(requestModel.Username);

        if (user == null)
        {
            _logger.LogInformation($"User not found: {requestModel.Username}");
            return Result<LoginResponseModel>.ValidationError("Invalid username or password.");
        }

        string hashedPassword = requestModel.Password.HashPassword(requestModel.Username);

        bool isValid = hashedPassword == user.Password;

        if (!isValid)
        {
            _logger.LogInformation($"Invalid password for user: {requestModel.Username}");
            return Result<LoginResponseModel>.ValidationError("Invalid username or password.");
        }

        var token = _jwtService.GenerateToken(user.Admincode, user.Username);

        var refreshToken = GenerateUlid();
        var refreshTokenExpiresAt = DateTime.Now.AddDays(1);

        var adminCode = user.Admincode;
        if (string.IsNullOrWhiteSpace(adminCode))
            throw new InvalidOperationException("AdminCode cannot be null.");

        var sessionId = GenerateUlid();
        if (string.IsNullOrWhiteSpace(sessionId))
            throw new InvalidOperationException("SessionId cannot be null.");

        // Create a new login session in tbl_login
        var newLogin = new TblLogin
        {
            Loginid = GenerateUlid(),
            Admincode = adminCode,
            Username = user.Username,
            Sessionid = sessionId,
            Refreshtoken = refreshToken,
            Refreshtokenexpiresat = refreshTokenExpiresAt,
            Loginstatus = "Login",
            Createdby = "SYSTEM",
            Createdat = DateTime.Now,
            Deleteflag = false
        };

        await _daAuth.CreateLogin(newLogin);

        _logger.LogInformation($"Generated Token: {token}");

        return Result<LoginResponseModel>.Success(new LoginResponseModel
        {
            Token = token,
            TokenExpiresAt = DateTime.Now.AddMinutes(_jwtSettings.TokenExpiryMinutes),
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
            RequirePasswordChange = user.Isfirsttime
        }, user.Isfirsttime? "First login. Please change your password." : "Login succeeded.");
    }
    
    private async Task<Result<string>> UpdatePassword(ChangePasswordRequestModel request, bool isFirstTimeLogin)
    {
        var user = await _daAuth.GetUserByUsername(request.Username);

        if (user == null)
            return Result<string>.ValidationError("User not found.");

        var isValid = string.Equals(
            request.CurrentPassword.HashPassword(request.Username),
            user.Password,
            StringComparison.Ordinal
        );
        if (!isValid)
        {
            return Result<string>.ValidationError("Current password is incorrect.");
        }

        var hashedPassword = request.NewPassword.HashPassword(request.Username);

        await _daAuth.UpdatePassword(request.Username, hashedPassword);

        if (isFirstTimeLogin && user.Isfirsttime)
        {
            await _daAuth.SetIsFirstTimeToFalse(request.Username);
        }

        return Result<string>.Success("Password updated successfully.");
    }
    
    public async Task<Result<string>> ChangePasswordFromFirstLogin(ChangePasswordRequestModel request)
    {
        return await UpdatePassword(request, isFirstTimeLogin: true);
    }
    
    public async Task<Result<string>> ChangePassword(ChangePasswordRequestModel request)
    {
        return await UpdatePassword(request, isFirstTimeLogin: false);
    }

    public async Task<Result<RefreshTokenResponseModel>> RefreshToken(RefreshTokenRequestModel request)
    {
        var login = await _daAuth.GetUserByRefreshToken(request.RefreshToken);

        if (login == null || login.Refreshtokenexpiresat < DateTime.Now)
        {
            return Result<RefreshTokenResponseModel>.ValidationError("Invalid or expired refresh token.");
        }

        if (login.Loginstatus == "Logout")
        {
            return Result<RefreshTokenResponseModel>.ValidationError("The user has been logged out.");
        }

        var newJwt = _jwtService.GenerateToken(login.Admincode, login.Username);

        // Check if refresh token is about to expire
        if ((login.Refreshtokenexpiresat - DateTime.Now).TotalDays < 1)
        {
            _logger.LogInformation($"Rotating refresh token for loginid: {login.Loginid}");
            login.Refreshtoken = GenerateUlid();
            login.Refreshtokenexpiresat = DateTime.Now.AddDays(1);

            // Update audit fields
            login.Modifiedby = "SYSTEM";
            login.Modifiedat = DateTime.Now;

            await _daAuth.UpdateLogin(login);
        }

        return Result<RefreshTokenResponseModel>.Success(new RefreshTokenResponseModel
        {
            Token = newJwt,
            TokenExpiresAt = DateTime.Now.AddMinutes(_jwtSettings.TokenExpiryMinutes),
            RefreshToken = login.Refreshtoken,
            RefreshTokenExpiresAt = login.Refreshtokenexpiresat
        }, "Token refreshed.");
    }

    public async Task<Result<string>> Logout(LogoutRequestModel request)
    {
        var login = await _daAuth.GetUserByRefreshToken(request.RefreshToken);

        if (login == null)
        {
            return Result<string>.ValidationError("Invalid refresh token.");
        }

        // Update login status as "logout", Update audit fields
        login.Loginstatus = "Logout";
        login.Refreshtokenexpiresat = DateTime.Now;
        login.Modifiedby = "SYSTEM";
        login.Modifiedat = DateTime.Now;

        await _daAuth.UpdateLogin(login);

        _logger.LogInformation($"User Logged out: {login.Loginid}");

        return Result<string>.Success("Logout succeeded.");
    }
}