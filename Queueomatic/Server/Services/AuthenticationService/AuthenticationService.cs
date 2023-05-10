using System.Security.Cryptography;
using System.Text;
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

	/// <summary>
	/// Registers a new user with the given password.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="password"></param>
	/// <returns>Whether or not the user was successfully registered</returns>
	public async Task<bool> Register(User user, string password)
	{
		if (await UnitOfWork.UserRepository.GetAsync(user.Email) != null)
		{
			return false;
		}
		
		CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
		user.PasswordHash = passwordHash;
		user.PasswordSalt = passwordSalt;
		
		await UnitOfWork.UserRepository.AddAsync(user);
		await UnitOfWork.SaveAsync();
		return true;
	}

	public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512();
		passwordSalt = hmac.Key;
		passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
	}

	public async Task<bool> CredentialsAreValid(string username, string password)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		throw new NotImplementedException();
	}
}