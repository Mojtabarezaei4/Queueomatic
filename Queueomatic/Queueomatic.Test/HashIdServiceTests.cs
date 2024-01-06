using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Queueomatic.Server.Services.HashIdService;

namespace Queueomatic.Test;

public class HashIdServiceTests
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

	[Fact]
	public void EncodeValue_ShouldReturnEmpty()
	{
		//Arrange
		var id = -1;
		var config = A.Fake<IConfiguration>();
		config.GetSection("HashIdKey").GetSection("Default").Value = "1234567890";

		var sut = new HashIdService(config);

		//Act
		var hashedId = sut.Encode(id);

		//Assert
		Assert.Equal("", hashedId);
	}

    [Theory]
	[InlineData("")]
	[InlineData("asfasg")]
	[InlineData("1123")]
	[InlineData("fthgmrtjymrmty")]
	[InlineData("zx132")]
	[InlineData("-1")]
	[InlineData("test")]
	[InlineData(".-,-.,")]
    public void DecodeMalformedValue_ShouldReturnNegativeNumber(string id)
    {
		//Arrange
        var config = A.Fake<IConfiguration>();
        config.GetSection("HashIdKey").GetSection("Default").Value = "1234567890";
        var sut = new HashIdService(config);

        //Act
		var result = sut.Decode(id);

        //Assert
		Assert.True(result < 0);
    }
}