using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.AuthenticationService;

public interface IAuthenticationService
{
    public Task<bool> Register(SignupDto user);
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public Task<bool> CredentialsAreValid(string email, string password);
	public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}