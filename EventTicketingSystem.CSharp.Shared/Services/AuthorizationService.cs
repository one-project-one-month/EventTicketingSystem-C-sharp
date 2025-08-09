using System.IdentityModel.Tokens.Jwt;

namespace EventTicketingSystem.CSharp.Shared.Services;

public abstract class AuthorizationService
{
    private readonly IHttpContextAccessor _contextAccessor;

    protected AuthorizationService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    private ApiTokenModel? GetUser()
    {
        var bearerToken = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(bearerToken) || !bearerToken.StartsWith("Bearer "))
        {
            return null;
        }

        var token = bearerToken["Bearer ".Length..];

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = handler.ReadJwtToken(token); 

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (userId is null)
                return null;

            return new ApiTokenModel{ UserId = userId };
        }
        catch
        {
            return null; 
        }
    }

    protected string CurrentUserId => GetUser()?.UserId ?? string.Empty;
}

public class ApiTokenModel
{
    public string? UserId { get; set; } = "";
}