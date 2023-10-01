namespace FTM.Domain.Services;

public interface IEmailService
{
    public Task<EmailResult> SendEmail(EmailRequest request);
}


public record EmailRequest(string Message, string Subject, string To);
public record EmailResult(bool Success, string? Error = null, Exception? Exception = null);