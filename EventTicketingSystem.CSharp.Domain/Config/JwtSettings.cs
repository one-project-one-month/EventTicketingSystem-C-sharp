namespace EventTicketingSystem.CSharp.Domain.Config;

public class JwtSettings
{
    [Required(ErrorMessage = "JwtSettings:SecretKey is required.")]
    public string SecretKey { get; set; } = null!; 

    [Required(ErrorMessage = "JwtSettings:Issuer is required.")]
    public string Issuer { get; set; } = null!;

    [Required(ErrorMessage = "JwtSettings:Audience is required.")]
    public string Audience { get; set; } = null!;

    [Range(1, 1440, ErrorMessage = "TokenExpiryMinutes must be between 1 and 1440.")]
    public int TokenExpiryMinutes { get; set; }
}