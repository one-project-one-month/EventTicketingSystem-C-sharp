namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Auth")]
[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly BL_Auth _blAuth;

    public AuthController(BL_Auth blAuth)
    {
        _blAuth = blAuth;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        var result = await _blAuth.Login(request);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel request)
    {
        var result = await _blAuth.ChangePassword(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("ChangePasswordFirstLogin")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordFirstLogin([FromBody] ChangePasswordRequestModel request)
    {
        var result = await _blAuth.ChangePasswordFromFirstLogin(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestModel request)
    {
        var result = await _blAuth.RefreshToken(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }
    
    [HttpPost("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestModel request)
    {
        var result = await _blAuth.Logout(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }
}