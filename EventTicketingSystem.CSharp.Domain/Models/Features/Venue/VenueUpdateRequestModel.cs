namespace EventTicketingSystem.CSharp.Domain.Models.Features.Venue;

public class VenueUpdateRequestModel
{
    [Required]
    public required string VenueCode { get; set; } 
    
    public string? Address { get; set; }             

    public string? Description { get; set; }         

    public string? Facilities { get; set; }

    public List<string>? Addons { get; set; }

    public List<IFormFile>? VenueImage { get; set; }
}