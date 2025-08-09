namespace EventTicketingSystem.CSharp.Shared.Models;

public class EmailModel
{
    public string Email { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
}

public class AttachmentEmailModel : EmailModel
{
    public string? RecipientName { get; set; }

    public List<string>? AttachmentPaths { get; set; }
}