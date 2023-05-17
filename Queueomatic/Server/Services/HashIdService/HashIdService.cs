using HashidsNet;

namespace Queueomatic.Server.Services.HashIdService;

public class HashIdService : IHashIdService
{
	private readonly Hashids _hashids;

	public HashIdService(IConfiguration configuration)
	{
		var _salt = configuration.GetSection("HashIdKey").GetSection("Default").Value;
		_hashids = new Hashids(_salt, 6);
	}

	public string Encode(int id)
	{
		var hash = _hashids.Encode(id);
		return hash;
	}

	public int Decode(string hash)
	{
		return _hashids.DecodeSingle(hash);
	}
}