using FakeItEasy;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.RoomService;

namespace Queueomatic.Test;

public class RoomService_Tests
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
}