using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Test;

public class RoomRepository_ReturnCorrectValues
{
    [Fact]
    public async Task GetRoom_ReturnRoomOrNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "QueueomaticTestDatabase")
            .Options;

        var room = A.Fake<Room>();
        var userRepository = A.Fake<IUserRepository>();
        var roomRepository = A.Fake<IRoomRepository>();
        A.CallTo(() => roomRepository.GetAsync(room.Id)).Returns(room);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, userRepository, roomRepository);
        var result = await sut.RoomRepository.GetAsync(room.Id);

        //Assert
        Assert.Equivalent(room, result);
    }
}