using System.Security.Cryptography;
using System.Text;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;

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
    /// <param name="userSignup"></param>
    /// <returns>Whether or not the user was successfully registered</returns>
    public async Task<bool> Register(SignupDto userSignup)
	{
		if (await UnitOfWork.UserRepository.GetAsync(userSignup.Email) != null)
		{
			return false;
		}
		
		CreatePasswordHash(userSignup.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Email = userSignup.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            NickName = userSignup.NickName
        };

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

	/// <summary>
	/// Checks if the given credentials are valid.
	/// </summary>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public async Task<bool> CredentialsAreValid(string email, string password)
	{
		var user = await UnitOfWork.UserRepository.GetAsync(email);
		return user != null && VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
	}

	public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512(passwordSalt);
		var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
		return computedHash.SequenceEqual(passwordHash);
	}
}