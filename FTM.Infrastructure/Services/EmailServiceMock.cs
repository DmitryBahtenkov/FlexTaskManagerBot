using System.Text.Json;
using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace FTM.Infrastructure.Services;

public class EmailServiceMock : IEmailService
{
    private readonly ILogger<IEmailService> _logger;

    public EmailServiceMock(ILogger<IEmailService> logger)
    {
        _logger = logger;
    }

    public async Task<EmailResult> SendEmail(EmailRequest request)
    {
        var host = Environment.GetEnvironmentVariable("SMTP_HOST");
        if (string.IsNullOrEmpty(host))
        {
            throw new BusinessException("Не указан хост для отправки Email");
        }
        
        var smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT");
        if (string.IsNullOrEmpty(smtpPort))
        {
            throw new BusinessException("Не указан порт SMTP-сервера");
        }

        var smtpLogin = Environment.GetEnvironmentVariable("SMTP_LOGIN");
        if (string.IsNullOrEmpty(smtpLogin))
        {
            throw new BusinessException("Не указан логин для SMTP-авторизации");
        }

        var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        if (string.IsNullOrEmpty(smtpPassword))
        {
            throw new BusinessException("Не указан пароль для SMTP-авторизации");
        }

        var smtpFrom = Environment.GetEnvironmentVariable("SMTP_FROM");
        if (string.IsNullOrEmpty(smtpFrom))
        {
            throw new BusinessException("Не указан адрес отправителя (SMTP_FROM)");
        }
        
        try
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(smtpFrom, smtpLogin));
            emailMessage.To.Add(new MailboxAddress(string.Empty, request.To));
            emailMessage.Subject = request.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = request.Message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(host, int.TryParse(smtpPort, out var p) ? p : 465, true);
                await client.AuthenticateAsync(smtpLogin, smtpPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }

            return new EmailResult(true);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error sendidng email");
            return new EmailResult(false, ex.Message, ex);
        }
    }
}