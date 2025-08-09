namespace EventTicketingSystem.CSharp.Domain.Models.Features.Auth;

public class ChangePasswordRequestModel
{
    public string Username { get; set; }
    
    public string CurrentPassword { get; set; }
    
    public string NewPassword { get; set; }
}