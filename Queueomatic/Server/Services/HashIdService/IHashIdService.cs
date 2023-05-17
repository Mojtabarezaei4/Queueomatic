namespace Queueomatic.Server.Services.HashIdService;

public interface IHashIdService
{
	public string Encode(int id);
	public int Decode(string hash);
}