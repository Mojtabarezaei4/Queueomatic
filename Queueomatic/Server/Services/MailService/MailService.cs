using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Queueomatic.DataAccess.Models;

namespace Queueomatic.Server.Services.MailService;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(Email model)
    {
        var mail = new MimeMessage();
        mail.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        mail.To.Add(MailboxAddress.Parse(model.ToEmail));
        mail.Subject = model.Subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = model.Body;
        mail.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(mail);
        smtp.Disconnect(true);
    }
}