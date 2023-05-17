using HashidsNet;

namespace Queueomatic.Server.Services.HashIdService;

public class HashIdService : IHashIdService
{
	private readonly Hashids _hashids;
	private readonly IConfiguration _configuration;

	public HashIdService(Hashids hashids, IConfiguration configuration)
	{
		_hashids = hashids;
		_configuration = configuration;
	}
	
	public string Encode(int id)
	{
		
	}

	public int Decode(string hash)
	{
		throw new NotImplementedException();
	}
}