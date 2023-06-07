using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.MailService;

public class MailService : IMailService
{
    private readonly MailSettingsDto _mailSettings;
    private readonly IConfiguration _configuration;
    public MailService(IOptions<MailSettingsDto> mailSettings, IConfiguration configuration)
    {
        _mailSettings = mailSettings.Value;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(EmailDto model, string mail)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
        email.To.Add(MailboxAddress.Parse(mail));
        email.Subject = model.Subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = model.Body;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);

        smtp.Disconnect(true);
    }

    public EmailDto CreateEmail(string email, User user)
    {
        var url = $"{_configuration.GetSection("MailSettings")["AppUrl"]}?token={user.PasswordResetToken}";

        return new EmailDto()
        {
            ToEmail = email,
            Subject = "Reset Password",
            Body = "<h1>Follow the instructions to reset your password</h1>" +
                   $"<p>To reset your password <a href='{url}' target='_blank'>Click here</a></p>"
        };
    }
}