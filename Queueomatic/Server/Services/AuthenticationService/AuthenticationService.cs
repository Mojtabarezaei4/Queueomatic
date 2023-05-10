using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Server.Services.AuthenticationService;

public class AuthenticationService : IAuthenticationService
{
	private readonly IUnitOfWork UnitOfWork;

	public AuthenticationService(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task<bool> Register(User user, string password)
	{
		throw new NotImplementedException();
	}

	public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> CredentialsAreValid(string username, string password)
	{
		throw new NotImplementedException();
	}
}