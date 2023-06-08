using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.MailService;

public interface IMailService
{
    public Task SendEmailAsync(EmailDto dto, string mail);
    public EmailDto CreateEmail(string email, User user);
}