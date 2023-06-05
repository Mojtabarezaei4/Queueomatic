using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Ocsp;
using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.MailService;

public class MailService : IMailService
{
    private readonly MailSettingsDto _mailSettings;
    private readonly IConfiguration _configuration;
    public MailService(IOptions<MailSettingsDto> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(EmailDto model)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("aglae16@ethereal.email"));
        email.To.Add(MailboxAddress.Parse("aglae16@ethereal.email"));
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

    public async Task<EmailDto> CreateEmail(string email, User user)
    {
        var url = $"{_configuration.GetSection("MailSettings")["AppUrl"]}/ResetPasswordDto?token={user.PasswordResetToken}";

        var mail = new EmailDto()
        {
            ToEmail = email,
            Subject = "Reset Password",
            Body = "<h1>Follow the instructions to reset your password</h1>" +
                   $"<p>To reset your password <a href='{url}' target='_blank'>Click here</a></p>"
        };
        return mail;
    }
}