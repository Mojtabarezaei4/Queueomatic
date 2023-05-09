using Queueomatic.DataAccess.Models;

namespace Queueomatic.Server.Services.AuthenticationService;

public interface IAuthenticationService
{
    public Task Register(User user);
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public Task<bool> UserCredentialsValid(string  username, string password);
}