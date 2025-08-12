namespace EventTicketingSystem.CSharp.Domain.Models.Features.VerificationCode;

public class VCRequestModel
{
    public string? VerificationCode { get; set; }

    public string? Email { get; set; }
}