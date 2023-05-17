using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Queueomatic.Server.Services.HashIdService;

namespace Queueomatic.Test;

public class HashIdService_Tests
{
	[Fact]
	public void EncodeValue_ReturnCorrectSequence()
	{
		//Arrange
		var id = 1;
		var config = A.Fake<IConfiguration>();
		config.GetSection("HashIdKey").GetSection("Default").Value = "1234567890";
		
		var sut = new HashIdService(config);

		//Act
		var hashedId = sut.Encode(id);
		var decodedId = sut.Decode(hashedId);

		//Assert
		Assert.Equal(id, decodedId);
	}
}