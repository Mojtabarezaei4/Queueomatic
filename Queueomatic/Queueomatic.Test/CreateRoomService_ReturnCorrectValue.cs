using FakeItEasy;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.RoomService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Test;

public class CreateRoomService_ReturnCorrectValue
{
    [Theory]
    [InlineData("12345678901234567890","test@test.com")]
    [InlineData("1sd45678901z34567as0","QWesa12@rqfas.asd.ces")]
    [InlineData("ASdz12345678901234567890","12test@test.com")]
    [InlineData("zxcSdz1234567890123456789asd","12test@test.com")]
    public async Task CreateRoomAsync_ReturnTrue(string roomName, string email)
    {
        // Arrange
        var uow = A.Fake<IUnitOfWork>();
		var hashIdService = A.Fake<IHashIdService>();

		// Act 
		var sut = new RoomService(uow, new Random(), hashIdService);

		var result = await sut.CreateRoomAsync(roomName, email);
        
        // Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("12345678901234567890","test@test.com")]
    [InlineData("1sd45678901","QWesa12@rqfas.asd.ces")]
    [InlineData("ASdz","12test@test.com")]
    [InlineData("z","12test@test.com")]
    public void NameOfRoomShouldBeLessThanTwentyCharacters_ReturnTrue(string roomName, string email)
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