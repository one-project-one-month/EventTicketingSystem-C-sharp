namespace EventTicketingSystem.CSharp.Shared.Services;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IFluentEmail _emailSender;

    public EmailService(ILogger<EmailService> logger, IFluentEmail emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task<bool> SendVerificationEmail(EmailModel requestModel)
    {
        try
        {
            var emailResult = await _emailSender
                                .To(requestModel.Email)
                               .Subject(requestModel.Subject)
                               .Body(requestModel.Body, isHtml: true)
                               .SendAsync();
            if (emailResult.Successful is false)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return false;
        }

        return true;
    }

    public async Task<bool> SendAttachmentEmail(AttachmentEmailModel requestModel)
    {
        try
        {
            var email = _emailSender
                .To(requestModel.Email, requestModel.RecipientName ?? string.Empty)
                .Subject(requestModel.Subject)
                .Body(requestModel.Body, isHtml: true);

            AttachFiles(email, requestModel.AttachmentPaths);

            var emailResult = await email.SendAsync();
            if (emailResult.Successful is false)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return false;
        }

        return true;
    }

    private void AttachFiles(IFluentEmail email, List<string>? attachmentPaths)
    {
        if (attachmentPaths is null || !attachmentPaths.Any())
        {
            return;
        }

        foreach (var path in attachmentPaths)
        {
            if (File.Exists(path))
            {
                email.AttachFromFilename(path, "image/jpeg");
            }
            else
            {
                _logger.LogWarning("Attachment file not found: {Path}", path);
            }
        }
    }
}