using HashidsNet;
using Microsoft.Extensions.Configuration;

namespace Queueomatic.Server.Services.HashIdService;

public class HashIdService : IHashIdService
{
	private readonly Hashids _hashids;
	private readonly string _salt;

	public HashIdService(Hashids hashids, IConfiguration configuration)
	{
		_hashids = hashids;
		_salt = configuration.GetSection("HashIdKey").GetSection("Default").Value;
	}

	public string Encode(int id)
	{

	}

	public int Decode(string hash)
	{
		throw new NotImplementedException();
	}
}