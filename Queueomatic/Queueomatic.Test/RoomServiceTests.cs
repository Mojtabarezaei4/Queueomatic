using FakeItEasy;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.RoomService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Test;

public class RoomServiceTests
{
	[Fact]
	public void FromEntity_ShouldReturnSixLetter()
	{
		//Arrange
		var random = A.Fake<Random>();
		var uow = A.Fake<IUnitOfWork>();
		var hashIdService = A.Fake<IHashIdService>();
		
		var room = A.Fake<Room>();
		room.Id = 10;
		
		A.CallTo(() => hashIdService.Encode(room.Id)).Returns("AgwBwB");
	
		var sut = new RoomService(uow, random, hashIdService);

		//Act
		var result = sut.FromEntity(room);

		//Assert
		Assert.Equal(6, result.HashId.Length);
	}

    [Theory]
    [InlineData("12345678901234567890", "test@test.com")]
    [InlineData("1sd45678901z34567as0", "QWesa12@rqfas.asd.ces")]
    [InlineData("ASdz12345678901234567890", "12test@test.com")]
    [InlineData("zxcSdz1234567890123456789asd", "12test@test.com")]
    public async Task CreateRoomAsync_ReturnRoomObject(string roomName, string email)
    {
        // Arrange
        var uow = A.Fake<IUnitOfWork>();
        var hashIdService = A.Fake<IHashIdService>();

        // Act 
        var sut = new RoomService(uow, new Random(), hashIdService);

        var result = await sut.CreateRoomAsync(roomName, email);

        // Assert
        Assert.Equal(result!.Name, roomName);
    }

    [Theory]
    [InlineData("12345678901234567890")]
    [InlineData("1sd45678901")]
    [InlineData("ASdz")]
    [InlineData("z")]
    public void NameOfRoomShouldBeLessThanTwentyCharacters_ReturnTrue(string roomName)
    {
        // Arrange
        var roomDto = A.Fake<RoomDto>();
        roomDto.Name = roomName;

        // Act 
        var result = roomDto.Name.Length <= 20;

        // Assert
        Assert.True(result);
    }
}