using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Test;

public class ParticipantRepository_ReturnCorrectValue
{
    [Fact]
    public async Task GetParticipant_ReturnParticipantOrNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "QueueomaticTestDatabase")
            .Options;

        var participant = A.Fake<Participant>();
        var participantRepository = A.Fake<IParticipantRepository>();
        var userRepository = A.Fake<IUserRepository>();
        var roomRepository = A.Fake<IRoomRepository>();
        A.CallTo(() => participantRepository.GetAsync(participant.Id)).Returns(participant);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, participantRepository, userRepository, roomRepository);
        var result = await sut.ParticipantRepository.GetAsync(participant.Id);

        //Assert
        Assert.Equivalent(participant, result);
    }

    [Fact]
    public async Task GetAllParticipants_ReturnTotalParticipant()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "QueueomaticTestDatabase")
            .Options;

        var participants = A.Fake<IEnumerable<Participant>>();
        var participantRepository = A.Fake<IParticipantRepository>();
        var userRepository = A.Fake<IUserRepository>();
        var roomRepository = A.Fake<IRoomRepository>();
        A.CallTo(() => participantRepository.GetAllAsync()).Returns(participants);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, participantRepository, userRepository, roomRepository);
        var result = await sut.RoomRepository.GetAllAsync();

        //Assert
        Assert.True(participants.Count() == result.Count());
    }
}