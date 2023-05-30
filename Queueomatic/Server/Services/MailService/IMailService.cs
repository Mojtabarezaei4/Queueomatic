using Queueomatic.DataAccess.Models;

namespace Queueomatic.Server.Services.MailService;

public interface IMailService
{
    Task SendEmailAsync(Email model);
}