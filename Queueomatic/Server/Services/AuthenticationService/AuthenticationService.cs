using Queueomatic.DataAccess.Models;

namespace Queueomatic.Server.Services.AuthenticationService;

public class AuthenticationService : IAuthenticationService
{
    public async Task Register(User user)
    {
        throw new NotImplementedException();
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AreUserCredentialsValid(string username, string password)
    {
        throw new NotImplementedException();
    }
}