using Queueomatic.DataAccess.Models;

namespace Queueomatic.Server.Services.AuthenticationService;

public interface IAuthenticationService
{
    public Task<bool> Register(User user, string password);
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public Task<bool> CredentialsAreValid(string username, string password);
}