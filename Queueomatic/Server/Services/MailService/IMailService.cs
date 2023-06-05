using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.MailService;

public interface IMailService
{
    public Task SendEmailAsync(EmailDto dto);
    public Task<EmailDto> CreateEmail(string email, User user);
}