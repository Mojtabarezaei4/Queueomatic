using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Test;

public class RoomRepositoryTests
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
        var participantRepository = A.Fake<IParticipantRepository>();
        A.CallTo(() => roomRepository.GetAsync(room.Id)).Returns(room);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, participantRepository, userRepository, roomRepository);
        var result = await sut.RoomRepository.GetAsync(room.Id);

        //Assert
        Assert.Equivalent(room, result);
    }

    [Fact]
    public async Task GetAllRooms_ReturnTotalRoom()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "QueueomaticTestDatabase")
            .Options;

        var rooms = A.Fake<IEnumerable<Room>>();
        var userRepository = A.Fake<IUserRepository>();
        var roomRepository = A.Fake<IRoomRepository>();
        var participantRepository = A.Fake<IParticipantRepository>();
        A.CallTo(() => roomRepository.GetAllAsync()).Returns(rooms);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, participantRepository, userRepository, roomRepository);
        var result = await sut.RoomRepository.GetAllAsync();

        //Assert
        Assert.True(rooms.Count() == result.Count());
    }
}